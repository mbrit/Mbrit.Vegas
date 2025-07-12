enum PlayMode {
  goForBroke('Go For Broke'),
  make50PercentProfit('Make 50% Profit'),
  balanced('Balanced'),
  doubleYourMoney('Double Your Money');

  const PlayMode(this.displayName);
  final String displayName;
}
