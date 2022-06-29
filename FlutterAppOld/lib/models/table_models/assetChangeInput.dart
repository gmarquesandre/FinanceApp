class AssetChangeInput{

  String assetCode;
  DateTime dateStart;



  AssetChangeInput({
    required this.assetCode,
    required this.dateStart,

  });
  Map<String, dynamic> toJson() =>
      {
        'assetCode' : assetCode,
        'dateStart': dateStart.toString()

      };
}