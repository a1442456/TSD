import 'package:tiq/models/dtos/FacilityConfigEditModel.dart';
import 'package:tiq/models/dtos/PacHeadDto.dart';
import 'package:tiq/models/dtos/PurchaseTaskDto.dart';
import 'package:tiq/models/dtos/PurchaseTaskLineDto.dart';
import 'package:tiq/models/dtos/PurchaseTaskUpdateDto.dart';
import 'package:tiq/models/dtos/UserTokenResp.dart';
import 'package:tiq/models/dtos/ViewModelSimple.dart';
import 'package:tiq/models/rpc/RpcError.dart';

class RpcResponse<T> {
  T data;
  List<RpcError> errors;

  RpcResponse(this.data, this.errors);

  factory RpcResponse.fromJson(Map<String, dynamic> json, [String typeToDeserialize]) {
    var response = RpcResponse<T>(
      null,
      _parseErrors(json)
    );

    if (T == DateTime) {
      response.data = DateTime.fromMillisecondsSinceEpoch(json['data'], isUtc: true) as T;
      return response;
    }

    if (typeToDeserialize == 'List<ViewModelSimple>') {
      final data = List<ViewModelSimple>();
      (json['data'] as Iterable<dynamic>)
          .forEach((dynamic model) => data.add(ViewModelSimple.fromJson(model)));
      response.data = data as T;

      return response;
    }

    if (typeToDeserialize == 'List<String>') {
      final data = List<String>();
      (json['data'] as Iterable<dynamic>)
        .forEach((dynamic model) => data.add(model));
      response.data = data as T;

      return response;
    }

    if (typeToDeserialize == 'List<PurchaseTaskDto>') {
      final data = List<PurchaseTaskDto>();
      (json['data'] as Iterable<dynamic>)
          .forEach((dynamic model) => data.add(PurchaseTaskDto.fromJson(model)));
      response.data = data as T;

      return response;
    }

    if (T == UserTokenResp) {
      response.data = UserTokenResp.fromJson(json['data']) as T;
      return response;
    }

    if (T == PacHeadDto) {
      response.data = PacHeadDto.fromJson(json['data']) as T;
      return response;
    }

    if (T == FacilityConfigEditModel) {
      response.data = FacilityConfigEditModel.fromJson(json['data']) as T;
      return response;
    }

    if (T == PurchaseTaskUpdateDto) {
      response.data = PurchaseTaskUpdateDto.fromJson(json['data']) as T;
      return response;
    }

    if (T == PurchaseTaskLineDto) {
      response.data = PurchaseTaskLineDto.fromJson(json['data']) as T;
      return response;
    }

    response.data = json['data'] as T;
    return response;
  }

  static List<RpcError> _parseErrors(Map<String, dynamic> json) {
    final errors = List<RpcError>();
    if (json['errors'] != null) {
      json['errors'].forEach((e) {
        errors.add(RpcError.fromJson(e));
      });
    }

    return errors;
  }
}