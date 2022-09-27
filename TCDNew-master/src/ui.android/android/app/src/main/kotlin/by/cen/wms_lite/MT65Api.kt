package by.cen.tiq

import android.content.BroadcastReceiver
import android.content.Context
import android.content.Intent
import io.flutter.plugin.common.BinaryMessenger
import io.flutter.plugin.common.MethodCall
import io.flutter.plugin.common.MethodChannel

class MT65Api(private val ctx: Context, messenger: BinaryMessenger) : BroadcastReceiver() {
    private val mt65ToNativeMethodChannelName = "wms.bwd.by/mt65/2n"
    private val mt65ToDartMethodChannelName = "wms.bwd.by/mt65/2d"

    private val mt65ToNativeChannel: MethodChannel
    private val mt65ToDartChannel: MethodChannel

    init {
        this.mt65ToNativeChannel = MethodChannel(messenger, mt65ToNativeMethodChannelName)
        this.mt65ToDartChannel = MethodChannel(messenger, mt65ToDartMethodChannelName)
        mt65ToNativeChannel.setMethodCallHandler { call, result -> this.methodCallHandler(call, result) }
    }

    private fun methodCallHandler(call: MethodCall, result: MethodChannel.Result) {
        // Note: this method is invoked on the main thread.
        when {
            call.method == "scannerSetup" -> {
                this.scannerSetup()
                result.success(null)
            }
            call.method == "scannerActivate" -> {
                this.scannerActivate()
                result.success(null)
            }
            else -> result.notImplemented()
        }
    }

    private fun scannerSetup() {
        val intent = Intent("ACTION_BAR_SCANCFG")
        intent.putExtra("EXTRA_SCAN_MODE", 3)
        intent.putExtra("EXTRA_SCAN_AUTOENT", 0)
        this.ctx.sendBroadcast(intent)
    }

    private fun scannerActivate() {
        val intent = Intent("nlscan.action.SCANNER_TRIG")
        this.ctx.sendBroadcast(intent)
    }

    override fun onReceive(context: Context, intent: Intent) {
        if (intent.action == "nlscan.action.SCANNER_RESULT") {
            val isSuccessful = intent.getStringExtra("SCAN_STATE") == "ok"
            val barcode1 = intent.getStringExtra("SCAN_BARCODE1")
            val barcode2 = intent.getStringExtra("SCAN_BARCODE2")


            val barcode1Json: String
            if (barcode1 == null) {
                barcode1Json = "null"
            } else {
                barcode1Json = "\"$barcode1\""
            }

            val barcode2Json: String
            if (barcode2 == null) {
                barcode2Json = "null"
            } else {
                barcode2Json = "\"$barcode2\""
            }

            this.mt65ToDartChannel.invokeMethod(
                "scannerScan",
                "{\"isSuccessful\": $isSuccessful, \"barcode1\": $barcode1Json, \"barcode2\": $barcode2Json}"
            )
        }
    }
}