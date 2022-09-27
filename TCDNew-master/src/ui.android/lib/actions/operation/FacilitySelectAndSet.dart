import 'package:flutter/widgets.dart';
import 'package:tiq/actions/data/settings/FacilityConfigGet.dart';
import 'package:tiq/actions/ui/settings/FacilitySelect.dart';
import 'package:tiq/models/state/SettingsFacility.dart';
import 'package:tiq/services/GStateProvider.dart';

class FacilitySelectAndSet {
  Future<bool> run(BuildContext context) async {
    var result = false;

    final selectedFacilityId = await FacilitySelect().run(context);
    if (selectedFacilityId != null) {
      final facilityConfig = await FacilityConfigGet().run(context, selectedFacilityId);
      if (facilityConfig != null) {
        final settingsFacility = SettingsFacility(
          selectedFacilityId,
          facilityConfig.acceptanceProcessType,
          facilityConfig.palletCodePrefix,
          facilityConfig.isAcceptanceByPapersEnabled
        );

        GStateProvider.instance.setSettingsFacility(settingsFacility);
        result = true;
      }
    }

    return result;
  }
}
