import 'package:intl/intl.dart';
import 'package:flutter/material.dart';
import 'package:tiq/models/dtos/PurchaseTaskLineDto.dart';

class PurchaseTaskLineCard extends StatelessWidget {
  final nf = new NumberFormat('0.000');
  final df = DateFormat('dd.MM.yyyy');
  final PurchaseTaskLineDto _purchaseTaskLine;

  PurchaseTaskLineCard(this._purchaseTaskLine);

  @override
  Widget build(BuildContext context) {
    double nameContainerWidth = MediaQuery.of(context).size.width - 12.0;
    var barcode = _purchaseTaskLine.product.barcodes.length > 0
        ? _purchaseTaskLine.product.barcodes.join(", ")
        : 'НЕТ';

    final qty = '${nf.format(_purchaseTaskLine.state.qtyFull)} / ${nf.format(_purchaseTaskLine.quantity)}';

    var expirationDate = '';
    if (_purchaseTaskLine.state.expirationDate != null) {
      final dateToDisplay = _purchaseTaskLine.state.expirationDate.add(Duration(days: _purchaseTaskLine.state.expirationDaysPlus));
      expirationDate = '${df.format(dateToDisplay)}';
    }

    return Container(
        padding: EdgeInsets.all(4.0),
        decoration: BoxDecoration(
          border: Border(
            left: BorderSide(color: getStateMarkColor(_purchaseTaskLine), width: 4.0),
            right: BorderSide(color: getAbcCategoryColor(_purchaseTaskLine), width: 8.0)
          )
        ),
        child: Column(
          children: <Widget>[
            Container(
              margin: EdgeInsets.only(bottom: 4.0),
              width: nameContainerWidth,
              child: Text(
                _purchaseTaskLine.product.name,
                textAlign: TextAlign.left,
                style: TextStyle(color: Colors.blueAccent),
              ),
            ),
            Container(
              margin: EdgeInsets.only(bottom: 4.0),
              width: nameContainerWidth,
              child: Text(
                barcode,
                textAlign: TextAlign.left,
                style: TextStyle(color: Colors.blueAccent),
              ),
            ),
            Container(
              child: Row(
                children: <Widget>[
                  Expanded(
                    flex: 1,
                    child: Text(
                      qty,
                      textAlign: TextAlign.left,
                      style: TextStyle(color: Colors.redAccent),
                    ),
                  ),
                  Expanded(
                    flex: 1,
                    child: Text(
                      expirationDate,
                      textAlign: TextAlign.right,
                    ),
                  ),
                ],
              ),
            ),
          ],
        ),
    );
  }

  Color getAbcCategoryColor(PurchaseTaskLineDto purchaseTaskLine) {
    return
      purchaseTaskLine.product.abc == "b"
        ? Colors.cyanAccent
        : purchaseTaskLine.product.abc == "c"
          ? Colors.purpleAccent
          : Colors.transparent;
  }

  Color getStateMarkColor(PurchaseTaskLineDto purchaseTaskLine) {
    var result = Colors.white;
    if (purchaseTaskLine.quantity == purchaseTaskLine.state.qtyFull)
      result = Colors.greenAccent;
    else if (purchaseTaskLine.quantity > 0 && purchaseTaskLine.state.qtyFull != 0)
      result = Colors.yellowAccent;
    else if (purchaseTaskLine.quantity > 0 && purchaseTaskLine.state.qtyFull == 0)
      result = Colors.redAccent;

    return result;
  }
}
