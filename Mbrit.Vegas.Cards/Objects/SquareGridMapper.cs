using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Cards
{
    using System;
    using System.Drawing;

    /// <summary>
    /// Represents a point in a grid coordinate system
    /// </summary>
    public class GridPoint
    {
        public int X { get; set; }
        public int Y { get; set; }

        public GridPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public GridPoint() : this(0, 0) { }

        public override bool Equals(object obj)
        {
            if (obj is GridPoint other)
            {
                return X == other.X && Y == other.Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override string ToString()
        {
            return $"GridPoint({X}, {Y})";
        }
    }

    /// <summary>
    /// Manages a square grid mapped within a rectangle with guaranteed square cells
    /// </summary>
    public class SquareGridMapper
    {
        private Rectangle containerBounds;
        private RectangleF actualGridBounds;
        private int columns;
        private int rows;
        private float cellSize;
        private PointF gridOffset;

        public Rectangle ContainerBounds => containerBounds;
        public RectangleF ActualGridBounds => actualGridBounds;
        public int Columns => columns;
        public int Rows => rows;
        public float CellSize => cellSize;
        public PointF GridOffset => gridOffset;

        public enum FitMode
        {
            /// <summary>
            /// Fit the grid inside the rectangle (may have empty space)
            /// </summary>
            FitInside,
            /// <summary>
            /// Fill the rectangle completely (grid may extend beyond bounds)
            /// </summary>
            FillRectangle,
            /// <summary>
            /// Use fixed cell size regardless of bounds
            /// </summary>
            FixedCellSize
        }

        public SquareGridMapper(Rectangle bounds, int columns, int rows, FitMode fitMode = FitMode.FitInside)
        {
            if (columns <= 0 || rows <= 0)
                throw new ArgumentException("Grid must have positive columns and rows");

            this.containerBounds = bounds;
            this.columns = columns;
            this.rows = rows;

            RecalculateGrid(fitMode);
        }

        /// <summary>
        /// Creates a grid with a specific fixed cell size
        /// </summary>
        public SquareGridMapper(Rectangle bounds, float cellSize)
        {
            if (cellSize <= 0)
                throw new ArgumentException("Cell size must be positive");

            this.containerBounds = bounds;
            this.cellSize = cellSize;

            // Calculate how many cells fit
            this.columns = Math.Max(1, (int)(bounds.Width / cellSize));
            this.rows = Math.Max(1, (int)(bounds.Height / cellSize));

            RecalculateGrid(FitMode.FixedCellSize);
        }

        private void RecalculateGrid(FitMode fitMode)
        {
            switch (fitMode)
            {
                case FitMode.FitInside:
                    CalculateFitInside();
                    break;
                case FitMode.FillRectangle:
                    CalculateFillRectangle();
                    break;
                case FitMode.FixedCellSize:
                    CalculateFixedSize();
                    break;
            }
        }

        private void CalculateFitInside()
        {
            // Calculate cell size based on the most constraining dimension
            float maxCellWidth = (float)containerBounds.Width / columns;
            float maxCellHeight = (float)containerBounds.Height / rows;

            // Use the smaller dimension to ensure squares fit
            cellSize = Math.Min(maxCellWidth, maxCellHeight);

            // Calculate actual grid size
            float gridWidth = cellSize * columns;
            float gridHeight = cellSize * rows;

            // Center the grid within the container
            gridOffset = new PointF(
                containerBounds.X + (containerBounds.Width - gridWidth) / 2f,
                containerBounds.Y + (containerBounds.Height - gridHeight) / 2f
            );

            actualGridBounds = new RectangleF(gridOffset.X, gridOffset.Y, gridWidth, gridHeight);
        }

        private void CalculateFillRectangle()
        {
            // Calculate cell size based on the least constraining dimension
            float maxCellWidth = (float)containerBounds.Width / columns;
            float maxCellHeight = (float)containerBounds.Height / rows;

            // Use the larger dimension to ensure full coverage
            cellSize = Math.Max(maxCellWidth, maxCellHeight);

            // Calculate actual grid size
            float gridWidth = cellSize * columns;
            float gridHeight = cellSize * rows;

            // Center the grid (it may extend beyond container)
            gridOffset = new PointF(
                containerBounds.X + (containerBounds.Width - gridWidth) / 2f,
                containerBounds.Y + (containerBounds.Height - gridHeight) / 2f
            );

            actualGridBounds = new RectangleF(gridOffset.X, gridOffset.Y, gridWidth, gridHeight);
        }

        private void CalculateFixedSize()
        {
            // Grid size is predetermined by cell size and count
            float gridWidth = cellSize * columns;
            float gridHeight = cellSize * rows;

            // Center the grid within the container
            gridOffset = new PointF(
                containerBounds.X + (containerBounds.Width - gridWidth) / 2f,
                containerBounds.Y + (containerBounds.Height - gridHeight) / 2f
            );

            actualGridBounds = new RectangleF(gridOffset.X, gridOffset.Y, gridWidth, gridHeight);
        }

        public void UpdateBounds(Rectangle newBounds, FitMode fitMode = FitMode.FitInside)
        {
            containerBounds = newBounds;
            RecalculateGrid(fitMode);
        }

        public void UpdateGridSize(int newColumns, int newRows, FitMode fitMode = FitMode.FitInside)
        {
            if (newColumns <= 0 || newRows <= 0)
                throw new ArgumentException("Grid must have positive columns and rows");

            columns = newColumns;
            rows = newRows;
            RecalculateGrid(fitMode);
        }

        /// <summary>
        /// Converts a grid point to pixel coordinates (center of the cell)
        /// </summary>
        public PointF GridToPixel(GridPoint gridPoint)
        {
            return GridToPixel(gridPoint.X, gridPoint.Y);
        }

        /// <summary>
        /// Converts grid coordinates to pixel coordinates (center of the cell)
        /// </summary>
        public PointF GridToPixel(int gridX, int gridY)
        {
            float pixelX = gridOffset.X + (gridX + 0.5f) * cellSize;
            float pixelY = gridOffset.Y + (gridY + 0.5f) * cellSize;
            return new PointF(pixelX, pixelY);
        }

        /// <summary>
        /// Converts a grid point to pixel coordinates (top-left corner of the cell)
        /// </summary>
        public PointF GridToPixelCorner(GridPoint gridPoint)
        {
            return GridToPixelCorner(gridPoint.X, gridPoint.Y);
        }

        /// <summary>
        /// Converts grid coordinates to pixel coordinates (top-left corner of the cell)
        /// </summary>
        public PointF GridToPixelCorner(int gridX, int gridY)
        {
            float pixelX = gridOffset.X + gridX * cellSize;
            float pixelY = gridOffset.Y + gridY * cellSize;
            return new PointF(pixelX, pixelY);
        }

        /// <summary>
        /// Gets the square rectangle for a specific grid cell
        /// </summary>
        public RectangleF GetCellSquare(GridPoint gridPoint)
        {
            return GetCellSquare(gridPoint.X, gridPoint.Y);
        }

        /// <summary>
        /// Gets the square rectangle for a specific grid cell
        /// </summary>
        public RectangleF GetCellSquare(int gridX, int gridY)
        {
            PointF corner = GridToPixelCorner(gridX, gridY);
            return new RectangleF(corner.X, corner.Y, cellSize, cellSize);
        }

        /// <summary>
        /// Converts pixel coordinates to grid coordinates
        /// </summary>
        public GridPoint PixelToGrid(PointF pixelPoint)
        {
            return PixelToGrid(pixelPoint.X, pixelPoint.Y);
        }

        /// <summary>
        /// Converts pixel coordinates to grid coordinates
        /// </summary>
        public GridPoint PixelToGrid(float pixelX, float pixelY)
        {
            int gridX = (int)((pixelX - gridOffset.X) / cellSize);
            int gridY = (int)((pixelY - gridOffset.Y) / cellSize);

            // Clamp to valid grid range
            gridX = Math.Max(0, Math.Min(columns - 1, gridX));
            gridY = Math.Max(0, Math.Min(rows - 1, gridY));

            return new GridPoint(gridX, gridY);
        }

        /// <summary>
        /// Checks if a pixel point is within the actual grid bounds
        /// </summary>
        public bool IsPointInGrid(PointF pixelPoint)
        {
            return actualGridBounds.Contains(pixelPoint);
        }

        /// <summary>
        /// Checks if a grid point is within valid bounds
        /// </summary>
        public bool IsValidGridPoint(GridPoint point)
        {
            return IsValidGridPoint(point.X, point.Y);
        }

        /// <summary>
        /// Checks if grid coordinates are within valid bounds
        /// </summary>
        public bool IsValidGridPoint(int x, int y)
        {
            return x >= 0 && x < columns && y >= 0 && y < rows;
        }

        /// <summary>
        /// Gets all neighboring grid points (8-directional)
        /// </summary>
        public GridPoint[] GetNeighbors(GridPoint point, bool includeDiagonals = true)
        {
            var neighbors = new System.Collections.Generic.List<GridPoint>();

            // Orthogonal neighbors
            int[] dx = { -1, 1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };

            for (int i = 0; i < 4; i++)
            {
                int nx = point.X + dx[i];
                int ny = point.Y + dy[i];
                if (IsValidGridPoint(nx, ny))
                    neighbors.Add(new GridPoint(nx, ny));
            }

            // Diagonal neighbors
            if (includeDiagonals)
            {
                int[] ddx = { -1, -1, 1, 1 };
                int[] ddy = { -1, 1, -1, 1 };

                for (int i = 0; i < 4; i++)
                {
                    int nx = point.X + ddx[i];
                    int ny = point.Y + ddy[i];
                    if (IsValidGridPoint(nx, ny))
                        neighbors.Add(new GridPoint(nx, ny));
                }
            }

            return neighbors.ToArray();
        }

        /// <summary>
        /// Draws the grid lines on a Graphics object
        /// </summary>
        public void DrawGrid(Graphics g, Pen pen)
        {
            // Draw vertical lines
            for (int col = 0; col <= columns; col++)
            {
                float x = gridOffset.X + col * cellSize;
                g.DrawLine(pen, x, gridOffset.Y, x, gridOffset.Y + rows * cellSize);
            }

            // Draw horizontal lines
            for (int row = 0; row <= rows; row++)
            {
                float y = gridOffset.Y + row * cellSize;
                g.DrawLine(pen, gridOffset.X, y, gridOffset.X + columns * cellSize, y);
            }
        }

        /// <summary>
        /// Draws the container bounds (for debugging)
        /// </summary>
        public void DrawContainerBounds(Graphics g, Pen pen)
        {
            g.DrawRectangle(pen, containerBounds);
        }

        /// <summary>
        /// Highlights a specific cell
        /// </summary>
        public void HighlightCell(Graphics g, GridPoint point, Brush brush)
        {
            if (IsValidGridPoint(point))
            {
                RectangleF cellRect = GetCellSquare(point);
                g.FillRectangle(brush, cellRect);
            }
        }
    }
}