using Mbrit.Vegas.Lens.Graph;
using Mbrit.Vegas.Simulator;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;

namespace Mbrit.Vegas.Lens
{
    public partial class Form1 : Form
    {
        //private string Chain { get; set; }
        //private float HouseEdge { get; set; }

        private IWinLoseDrawRoundsBucket Rounds { get; set; }

        private IEnumerable<WinLoseDrawType> WalkVectors { get; set; }
        private IEnumerable<float> WalkProfits { get; set; }
        private float WalkHouseEdge { get; set; }

        private const float WalkGameHouseEdge = (float)WalkGameDefaults.HouseEdge;

        private const int SingleRun = 1;
        private const int MultipleRun = 1000;

        private static IEnumerable<float> HouseEdges = new List<float>() { 0.015f, 0.025f, 0.035f, 0.05f, 0.08f, 0.09f, 0.10f,
            0.11f, 0.12f, 0.13f, 0.14f, 0.15f, 0.16f, 0.17f, 0.18f };

        private enum Mode
        {
            None = 0,
            ReachSpike0p5 = 1,
            StretchToSpike1 = 2,
            ReachSpike1 = 3,
        }

        public Form1()
        {
            InitializeComponent();
        }

        private float Unit => 15f;

        private void Form1_Load(object sender, EventArgs e)
        {
            this.ReportExceptions(() =>
            {
                foreach (var mode in Enum.GetValues(typeof(Mode)))
                    this.listMode.Items.Add(mode);
                this.listMode.SelectedIndex = 0;

                foreach (var houseEdge in HouseEdges)
                    this.listHouseEdges.Items.Add(new HouseEdgeViewItem(houseEdge));
                this.listHouseEdges.SelectedIndex = 0;

                const string biased = "Biased";
                this.buttonBiased1.Text = biased + " " + SingleRun;
                this.buttonBiasedN.Text = biased + " " + MultipleRun;

                const string unbiased = "Unbiased";
                this.buttonUnbiased1.Text = unbiased + " " + SingleRun;
                this.buttonUnbiasedN.Text = unbiased + " " + MultipleRun;

                this.SetBiasedExemplar();
            });
        }

        private void SetBiasedExemplar()
        {
            var chain = "wwlwwlwwllwwlwwwwlllwlwwwllllwlwlwllwwllwwwwwlllwllllllllwllwwwlllwlwlwlwlllwllllllwwllwwlllwwllwlwl";
            var rounds = WinLoseDrawRoundsBucket.Parse(chain, 0.015M, this.GetRandom());
            SetChain(rounds);
        }

        private void SetChain(IWinLoseDrawRoundsBucket rounds)
        {
            this.Rounds = rounds;
            this.RefreshView();
        }

        private void RefreshView()
        {
            if (this.Rounds == null)
                return;

            /*
            var wins = new StringBuilder();
            var losses = new StringBuilder();
            var vectors = new List<WinLoseDrawType>();
            var index = 0;
            var numWins = 0;
            var numLosses = 0;
            foreach (var c in this.Chain)
            {
                var asString = new string(c, 1);
                if (asString == WinLoseDrawExtender.WinKey)
                {
                    wins.Append("W");
                    losses.Append(" ");

                    vectors.Add(WinLoseDrawType.Win);
                    numWins++;
                }
                else if (asString == WinLoseDrawExtender.LoseKey)
                {
                    losses.Append("L");
                    wins.Append(" ");

                    vectors.Add(WinLoseDrawType.Lose);
                    numLosses++;
                }
                else
                    throw new NotSupportedException($"Cannot handle '{asString}'.");

                index++;
                if (index % 10 == 0)
                {
                    losses.Append(" ");
                    wins.Append(" ");
                }
            }

            this.labelWins.Text = wins.ToString();
            this.labelLosses.Text = losses.ToString();

            this.labelNumWins.Text = numWins.ToString();
            this.labelNumLosses.Text = numLosses.ToString();

            this.Vectors = vectors;
            */

            // run the game...
            if (this.DoWalkGame)
            {
                var walk = new WalkFoo();

                var rand = this.GetRandom();

                var rounds = WinLoseDrawRoundsBucket.GetWinLoseBucket(1, WalkGameDefaults.HandsPerRound, (decimal)WalkGameHouseEdge, rand);

                var results = walk.DoMagic(rounds, MultipleRun, new AutomaticWalkGamePlayer(), (index, bucket) =>
                {
                    return WalkGameDefaults.GetSetup(this.WalkGameMode, rounds, (int)this.Unit, 0);

                }, false);

                var result = results.First().Results.First();

                var profits = new List<float>();
                var vectors = new List<WinLoseDrawType>();

                var index = 0;
                while (true)
                {
                    if (!(result.PointOutcomes.ContainsKey(index)))
                        break;

                    profits.Add((float)result.PointOutcomes[index].Profit);
                    vectors.Add(rounds[0].GetResult(index));

                    index++;
                }

                this.WalkProfits = profits;
                this.WalkVectors = vectors;
                this.WalkHouseEdge = (float)rounds.HouseEdge;
            }
            else
            {
                this.WalkProfits = new List<float>();
                this.WalkVectors = new List<WinLoseDrawType>();
                this.WalkHouseEdge = 0;
            }

            // draw...
            this.panel1.Invalidate();
        }

        private float SelectedHouseEdge => ((HouseEdgeViewItem)this.listHouseEdges.SelectedItem).HouseEdge;

        private Mode SelectedMode => (Mode)this.listMode.SelectedItem;

        public bool DoWalkGame => this.SelectedMode != Mode.None;

        public WalkGameMode WalkGameMode
        {
            get
            {
                var mode = this.SelectedMode;
                if (mode == Mode.ReachSpike0p5)
                    return WalkGameMode.ReachSpike0p5;
                else if (mode == Mode.StretchToSpike1)
                    return WalkGameMode.StretchToSpike1;
                else if (mode == Mode.ReachSpike1)
                    return WalkGameMode.ReachSpike1;
                else
                    throw new NotSupportedException($"Cannot handle '{mode}'.");
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            this.ReportExceptions(() =>
            {
                var illustrations = this.GetHouseEdgeIllustrations();

                var generator = new GraphGenerator(this.panel1.ClientRectangle, this.Rounds, this.WalkVectors, this.WalkProfits,
                    this.WalkHouseEdge, this.Unit, illustrations, WalkGameDefaults.HandsPerRound, 14, this.checkShowBoxHands.Checked,
                    this.checkShowWedge.Checked);

                generator.Render(e.Graphics);
            });
        }

        private IEnumerable<HouseEdgeIllustration> GetHouseEdgeIllustrations()
        {
            var results = new List<HouseEdgeIllustration>();

            var dash = DashStyle.Dot;

            results.Add(new HouseEdgeIllustration(0.015f, Color.Red, dash));
            results.Add(new HouseEdgeIllustration(0.08f, Color.Red, dash));

            if (this.checkShowWeekday.Checked)
            {
                results.Add(new HouseEdgeIllustration(0.10f, Color.Orange, dash));
                results.Add(new HouseEdgeIllustration(0.12f, Color.Orange, dash));

                if (!(this.checkShowWeekend.Checked))
                    results.Add(new HouseEdgeIllustration(0.14f, Color.Orange, dash));
            }

            if (this.checkShowWeekend.Checked)
            {
                results.Add(new HouseEdgeIllustration(0.14f, Color.Magenta, dash));
                results.Add(new HouseEdgeIllustration(0.16f, Color.Magenta, dash));
                results.Add(new HouseEdgeIllustration(0.18f, Color.Magenta, dash));
            }

            return results;
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            this.ReportExceptions(() =>
            {
                this.panel1.Invalidate();
            });
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            this.ReportExceptions(() =>
            {
                this.panel1.Invalidate();
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.ReportExceptions(() =>
            {
                this.SetBiasedExemplar();
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.ReportExceptions(() =>
            {
                this.SetChain(0, SingleRun);
            });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.ReportExceptions(() =>
            {
                this.SetChain(this.SelectedHouseEdge, SingleRun);
            });
        }

        private void SetChain(float houseEdge, int count)
        {
            var rand = this.GetRandom();
            var rounds = WinLoseDrawRoundsBucket.GetWinLoseBucket(count, 100, (decimal)houseEdge, rand, RoundsBucketFlags.Exact);

            this.SetChain(rounds);
        }

        private Random GetRandom() => new Random(VegasRuntime.GetToken().GetHashCode());

        private void listMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ReportExceptions(() =>
            {
                this.RefreshView();
            });
        }

        private void labelLosses_Click(object sender, EventArgs e)
        {

        }

        private void buttonBiasedN_Click(object sender, EventArgs e)
        {
            this.ReportExceptions(() =>
            {
                this.SetChain(this.SelectedHouseEdge, MultipleRun);
            });
        }

        private void buttonUnbiasedN_Click(object sender, EventArgs e)
        {
            this.ReportExceptions(() =>
            {
                this.SetChain(0f, MultipleRun);
            });
        }

        private void listHouseEdges_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ReportExceptions(() =>
            {
                this.RefreshView();
            });
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) =>
            this.ReportExceptions(() => this.RefreshView());

        private void checkShowWeekday_CheckedChanged(object sender, EventArgs e) =>
            this.ReportExceptions(() => this.RefreshView());

        private void checkShowWeekend_CheckedChanged(object sender, EventArgs e) =>
            this.ReportExceptions(() => this.RefreshView());

        private void checkShowWedge_CheckedChanged(object sender, EventArgs e) =>
            this.ReportExceptions(() => this.RefreshView());
    }
}
