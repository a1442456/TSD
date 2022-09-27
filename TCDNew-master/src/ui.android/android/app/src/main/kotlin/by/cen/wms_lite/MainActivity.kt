package by.cen.tiq

import android.content.Intent
import android.content.IntentFilter
import android.net.Uri
import android.os.Bundle
import androidx.core.content.FileProvider
import io.flutter.embedding.android.FlutterActivity
import io.flutter.embedding.engine.FlutterEngine
import io.flutter.plugin.common.MethodCall
import io.flutter.plugin.common.MethodChannel
import io.flutter.plugins.GeneratedPluginRegistrant
import java.io.File

class MainActivity : FlutterActivity() {
    var updateMethodChannel: MethodChannel? = null
    var mt65if: IntentFilter? = null
    var mt65br: MT65Api? = null

    override fun configureFlutterEngine(flutterEngine: FlutterEngine) {
        GeneratedPluginRegistrant.registerWith(flutterEngine)

        this.updateMethodChannel = MethodChannel(flutterEngine.getDartExecutor().getBinaryMessenger(), "by.tiq/update")
        this.updateMethodChannel?.setMethodCallHandler { call, result -> this.methodCallHandler(call, result) }
        this.mt65if = IntentFilter("nlscan.action.SCANNER_RESULT")
        this.mt65br = MT65Api(this.applicationContext, flutterEngine.getDartExecutor().getBinaryMessenger())
        registerReceiver(mt65br, mt65if)
    }

    private fun methodCallHandler(call: MethodCall, result: MethodChannel.Result) {
        // Note: this method is invoked on the main thread.
        when {
            call.method == "updateApk" -> {
                val apkFilePath = call.argument<String>("apkFilePath") ?: ""
                this.updateApk(apkFilePath)
            }
            else -> result.notImplemented()
        }
    }

    private fun updateApk(apkFilePath: String) {
        val intent = Intent(Intent.ACTION_INSTALL_PACKAGE)
        val data: Uri = FileProvider.getUriForFile(context, BuildConfig.APPLICATION_ID.toString() + ".provider", File(apkFilePath))
        intent.setDataAndType(data, "application/vnd.android.package-archive")
        intent.setFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION or Intent.FLAG_ACTIVITY_CLEAR_TASK or Intent.FLAG_ACTIVITY_NEW_TASK)
        context.startActivity(intent)
    }

    override fun onDestroy() {
        unregisterReceiver(this.mt65br)

        super.onDestroy()
    }
}