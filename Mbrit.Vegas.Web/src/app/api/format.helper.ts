export class FormatHelper {
  static asPercent2(value: any): string {
    return FormatHelper.asPercentN(value, 2);
  }

  static asPercent1(value: any): string {
    return FormatHelper.asPercentN(value, 1);
  }

  static asPercent0(value: any): string {
    return FormatHelper.asPercentN(value, 0);
  }

  private static asPercentN(value: any, places: number): string {
    value = Number(value);
    return (value * 100).toFixed(places) + "%";
  }

  static asFixed2(value: any): string {
    return FormatHelper.asFixedN(value, 2);
  }

  static asFixed1(value: any): string {
    return FormatHelper.asFixedN(value, 1);
  }

  static asFixed0(value: any): string {
    return FormatHelper.asFixedN(value, 0);
  }

  private static asFixedN(value: any, places: number): string {
    value = Number(value);
    return value.toLocaleString('en-US', {
      minimumFractionDigits: places,
      maximumFractionDigits: places
    });
  }

  static asCurrency2(value: any): string {
    return FormatHelper.asCurrencyN(value, 2);
  }

  static asCurrency1(value: any): string {
    return FormatHelper.asCurrencyN(value, 1);
  }

  static asCurrency0(value: any): string {
    return FormatHelper.asCurrencyN(value, 0);
  }

  private static asCurrencyN(value: any, places: number): string {
    return "$" + FormatHelper.asFixedN(value, places);
  }
}
