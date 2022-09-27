import 'package:tiq/models/enums/AcceptanceProcessType.dart';

class FacilityConfigEditModel {
  AcceptanceProcessType acceptanceProcessType;
  String palletCodePrefix;
  bool isAcceptanceByPapersEnabled;

  FacilityConfigEditModel(
    this.acceptanceProcessType,
    this.palletCodePrefix,
    this.isAcceptanceByPapersEnabled
  );

  factory FacilityConfigEditModel.fromJson(Map<String, dynamic> json) {
    FacilityConfigEditModel data;

    if (json != null) {
      data = FacilityConfigEditModel(
          json['acceptanceProcessType'] == 1000
              ? AcceptanceProcessType.simple
              : json['acceptanceProcessType'] == 2000
                ? AcceptanceProcessType.palletized
                : throw FormatException('Incorrect JSON'),
          json['palletCodePrefix'],
          json['isAcceptanceByPapersEnabled']
      );
    }

    return data;
  }
}