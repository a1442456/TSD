import 'package:tiq/models/enums/AcceptanceProcessType.dart';

class SettingsFacility {
  final String facilityId;
  final AcceptanceProcessType acceptanceProcessType;
  final String palletCodePrefix;
  final bool isAcceptanceByPapersEnabled;

  SettingsFacility(this.facilityId, this.acceptanceProcessType, this.palletCodePrefix, this.isAcceptanceByPapersEnabled);
}