package com.tiwater.gameframework;

import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.os.Bundle;
import android.util.Log;

import org.json.JSONObject;

import java.util.HashMap;
import java.util.Set;

public class MainExtActivity extends UnityPlayerActivity {

  private HashMap<String, UnityBroadCastReceiver> receivers = new HashMap<String, UnityBroadCastReceiver>();

  protected void onCreate(Bundle savedInstanceState) {
    // 调用 UnityPlayerActivity.onCreate()
    super.onCreate(savedInstanceState);
    // 将调试消息打印至 logcat
    Log.d("MainExtActivity", "onCreate called!");
  }

  /**
   * Register an intent receiver for Unity. No matter how many listeners in Unity, one action will only have
   * one receiver in Android layer.
   * @param action
   */
  public void registerIntentUnityReceiver(String action){
    if(!receivers.containsKey(action)){
      Log.i("MainExtActivity", "registerIntentUnityReceiver!");
      //Only need to register 1 receiver for an action
      UnityBroadCastReceiver receiver = new UnityBroadCastReceiver();
      receivers.put(action, receiver);
      //Register the receiver
      IntentFilter intentFilter=new IntentFilter();
      intentFilter.addAction(action);
      registerReceiver(receiver, intentFilter);
    }
  }

  /**
   * Unregister the intent receiver for Unity
   * @param action
   */
  public void unRegisterIntentUnityReceiver(String action){
    if(receivers.containsKey(action)){
      //Unregister the receiver
      unregisterReceiver(receivers.get(action));
      //Remove the key
      receivers.remove(action);
    }
  }

  @Override
  protected void onDestroy() {
    super.onDestroy();
    //Unregister the receivers
    for(UnityBroadCastReceiver receiver : receivers.values()){
      unregisterReceiver(receiver);
    }
  }

  /**
   * The class to receive the intent for Unity and dispatch it to Unity layer
   */
  public class UnityBroadCastReceiver extends BroadcastReceiver {

    @Override
    public void onReceive(Context context, Intent intent) {
      try {
        //Fetch intent as Json
        JSONObject root = new JSONObject();
        root.put("Action", intent.getAction());
        if(intent.getExtras()!=null) {
          Set<String> keySet = intent.getExtras().keySet();
          if (keySet != null) {
            JSONObject extras = new JSONObject();
            for (String key : keySet) {
              extras.put(key, intent.getExtras().get(key));
            }
            root.put("Extras", extras);
          }
        }
        Log.d("MainExtActivity", root.toString());
        //Dispatch to Unity
        UnityPlayer.UnitySendMessage("GameManager", "OnIntent", root.toString());
      } catch (Exception ex){
        ex.printStackTrace();
        Log.e("MainExtActivity", ex.getMessage());
      }
    }
  }
}
