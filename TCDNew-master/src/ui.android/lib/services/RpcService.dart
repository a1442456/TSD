import 'dart:async';
import 'dart:convert';

import 'package:tiq/models/dtos/FacilityConfigEditModel.dart';
import 'package:tiq/models/dtos/PacHeadDto.dart';
import 'package:tiq/models/dtos/PurchaseTaskDto.dart';
import 'package:tiq/models/dtos/PurchaseTaskLineDto.dart';
import 'package:tiq/models/dtos/PurchaseTaskUpdateDto.dart';
import 'package:tiq/models/dtos/ByIdReq.dart';
import 'package:tiq/models/dtos/PacHeadReadByBarcodeReq.dart';
import 'package:tiq/models/dtos/PurchaseTaskCreateFromPacsReq.dart';
import 'package:tiq/models/dtos/PurchaseTaskFinishReq.dart';
import 'package:tiq/models/dtos/PurchaseTaskLineReadByBarcodeReq.dart';
import 'package:tiq/models/dtos/PurchaseTaskLineUpdatePostReq.dart';
import 'package:tiq/models/dtos/PurchaseTaskListReadByPersonReq.dart';
import 'package:tiq/models/dtos/PurchaseTaskUpdateReadReq.dart';
import 'package:tiq/models/dtos/ViewModelSimple.dart';
import 'package:tiq/models/exceptions/RpcException.dart';
import 'package:tiq/models/rpc/RpcResponse.dart';
import 'package:tiq/models/dtos/UserCredentials.dart';
import 'package:tiq/models/dtos/UserTokenResp.dart';
import 'package:tiq/common/Urls.dart';
import 'package:http/http.dart';

class RpcService {
  RpcService._internal();
  static final RpcService _singleton = new RpcService._internal();
  static RpcService get instance => _singleton;

  Future<UserTokenResp> userLogin(UserCredentials request) async {
    final requestBody = json.encode(request);
    final response = await this._request(Urls.instance.userLoginUrl, requestBody);
    final jsonResponse = json.decode(response.body);
    final r = RpcResponse<UserTokenResp>.fromJson(jsonResponse);
    if (r.errors != null) {
      if (r.errors.length > 0) {
        throw RpcException(r.errors);
      }
    }

    return r.data;
  }

  Future<List<ViewModelSimple>> facilityListSimpleReadByPerson(String bearer) async {
    final response = await this._request(Urls.instance.facilityListSimpleReadByPersonUrl, null, bearer: bearer);
    final jsonResponse = json.decode(response.body);
    final r = RpcResponse<List<ViewModelSimple>>.fromJson(jsonResponse, 'List<ViewModelSimple>');
    if (r.errors != null) {
      if (r.errors.length > 0) {
        throw RpcException(r.errors);
      }
    }

    return r.data;
  }

  Future<FacilityConfigEditModel> facilityConfigGet(ByIdReq request, String bearer) async {
    final requestBody = json.encode(request);
    final response = await this._request(Urls.instance.facilityConfigGetUrl, requestBody, bearer: bearer);
    final jsonResponse = json.decode(response.body);
    final r = RpcResponse<FacilityConfigEditModel>.fromJson(jsonResponse);
    if (r.errors != null) {
      if (r.errors.length > 0) {
        throw RpcException(r.errors);
      }
    }

    return r.data;
  }

  Future<PacHeadDto> pacHeadReadByBarcode(PacHeadReadByBarcodeReq request, String bearer) async {
    final requestBody = json.encode(request);
    final response = await this._request(Urls.instance.pacHeadReadByBarcodeUrl, requestBody, bearer: bearer);
    final jsonResponse = json.decode(response.body);
    final r = RpcResponse<PacHeadDto>.fromJson(jsonResponse);
    if (r.errors != null) {
      if (r.errors.length > 0) {
        throw RpcException(r.errors);
      }
    }

    return r.data;
  }

  Future<List<PurchaseTaskDto>> purchaseTaskListReadByPerson(PurchaseTaskListReadByPersonReq request, String bearer) async {
    final requestBody = json.encode(request);
    final response = await this._request(Urls.instance.purchaseTaskListForPersonUrl, requestBody, bearer: bearer);
    final jsonResponse = json.decode(response.body);
    final r = RpcResponse<List<PurchaseTaskDto>>.fromJson(jsonResponse, 'List<PurchaseTaskDto>');
    if (r.errors != null) {
      if (r.errors.length > 0) {
        throw RpcException(r.errors);
      }
    }

    return r.data;
  }

  Future<String> purchaseTaskCreateFromPacs(PurchaseTaskCreateFromPacsReq request, String bearer) async {
    final requestBody = json.encode(request);
    final response = await this._request(Urls.instance.purchaseTaskCreateFromPacsUrl, requestBody, bearer: bearer);
    final jsonResponse = json.decode(response.body);
    final r = RpcResponse<String>.fromJson(jsonResponse);
    if (r.errors != null) {
      if (r.errors.length > 0) {
        throw RpcException(r.errors);
      }
    }

    return r.data;
  }

  Future<PurchaseTaskUpdateDto> purchaseTaskUpdateRead(PurchaseTaskUpdateReadReq request, String bearer) async {
    final requestBody = json.encode(request);
    final response = await this._request(Urls.instance.purchaseTaskUpdateReadUrl, requestBody, bearer: bearer);
    final jsonResponse = json.decode(response.body);
    final r = RpcResponse<PurchaseTaskUpdateDto>.fromJson(jsonResponse);
    if (r.errors != null) {
      if (r.errors.length > 0) {
        throw RpcException(r.errors);
      }
    }

    return r.data;
  }

  Future<PurchaseTaskLineDto> purchaseTaskLineReadByBarcode(PurchaseTaskLineReadByBarcodeReq request, String bearer) async {
    final requestBody = json.encode(request);
    final response = await this._request(Urls.instance.purchaseTaskLineReadByBarcodeUrl, requestBody, bearer: bearer);
    final jsonResponse = json.decode(response.body);
    final r = RpcResponse<PurchaseTaskLineDto>.fromJson(jsonResponse);
    if (r.errors != null) {
      if (r.errors.length > 0) {
        throw RpcException(r.errors);
      }
    }

    return r.data;
  }

  Future<bool> purchaseTaskLineUpdatePost(PurchaseTaskLineUpdatePostReq request, String bearer) async {
    final requestBody = json.encode(request);
    final response = await this._request(Urls.instance.purchaseTaskLineUpdatePostUrl, requestBody, bearer: bearer);
    final jsonResponse = json.decode(response.body);
    final r = RpcResponse<bool>.fromJson(jsonResponse);
    if (r.errors != null) {
      if (r.errors.length > 0) {
        throw RpcException(r.errors);
      }
    }

    return r.data;
  }

  Future<bool> purchaseTaskFinish(PurchaseTaskFinishReq request, String bearer) async {
    final requestBody = json.encode(request);
    final response = await this._request(Urls.instance.purchaseTaskFinishUrl, requestBody, bearer: bearer);
    final jsonResponse = json.decode(response.body);
    final r = RpcResponse<bool>.fromJson(jsonResponse);
    if (r.errors != null) {
      if (r.errors.length > 0) {
        throw RpcException(r.errors);
      }
    }

    return r.data;
  }

  Future<DateTime> timeRead() async {
    final response = await this._request(Urls.instance.timeReadUrl, null);
    final jsonResponse = json.decode(response.body);
    final r = RpcResponse<DateTime>.fromJson(jsonResponse);
    if (r.errors != null) {
      if (r.errors.length > 0) {
        throw RpcException(r.errors);
      }
    }

    return r.data;
  }

  Future<Response> _request(String url, String body, {String bearer = '', int timeout = 45}) async {
    Map<String, String> headers = {
      'Content-type': 'application/json',
      'Accept': 'application/json',
    };
    if (bearer != '') {
      headers['Authorization'] = 'Bearer $bearer';
    }
    final responseFuture =
      post(
          Uri.parse(url),
          headers: headers,
          body: body
      )
      .timeout(new Duration(seconds: timeout));

    return responseFuture;
  }
}
