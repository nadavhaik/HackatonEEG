
using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using UnityEngine;

[System.Serializable]
public class StressDataResponse
{
    public bool connected;
    public double? stress_gauge;
}


public class StressDataBridge : MonoBehaviour
{
    private static int PORT = 9400;
    private static int SYNC_INTERVAL_MILLIS = 500;
    private static readonly StressDataBridge instance = new StressDataBridge();
    private bool shouldRun;
    private StressDataBridge()    
    {    
        shouldRun = true;
    }

    public void Kill()
    {
        shouldRun = false;
    }

    public async void UpdateData()
    {
        var client = new HttpClient();
        var uri = new Uri("http://127.0.0.1:" + PORT + "/get/braindata");
        Stream respStream = await client.GetStreamAsync(uri);
        var bodyStream = new StreamReader(respStream);
        string res = bodyStream.ReadToEnd();
        Console.WriteLine(res);
        if(res != "null")
        {
            string[] parameters = res.Split('|');
            double gauge = Convert.ToDouble(parameters[0]);
            long t = ((DateTimeOffset)DateTime.Parse(parameters[1])).ToUnixTimeSeconds();
            if (StressData.GetInstance().SetLastUpdateTimeIfBigger(t))
                StressData.GetInstance().SetStressGauge(gauge);
        }
    }

    public void Run()
    {
        while (shouldRun)
        {
            this.UpdateData();
            StressData.GetInstance().markChanged();
            Thread.Sleep(SYNC_INTERVAL_MILLIS);
        }
         var client = new HttpClient();
         var uri = new Uri("http://127.0.0.1:" + PORT + "/kill");
         client.GetStreamAsync(uri);
    }
    public static StressDataBridge GetInstance()
    {
        return instance;
    }

}

public class StressData    
{
    private double? stressGauge;    
    private long lastUpdateTime;    
    private double stress_data;
    private static readonly StressData instance = new StressData();        
    private bool changed;

    private StressData()    
    {    
        last_time_updated_stress_level = Time.realtimeSinceStartup;
        stress_level = 0.0;
        changed = false;

        this.stressGauge = null;
        this.lastUpdateTime = 0;

        Thread thread1 = new Thread(StressDataBridge.GetInstance().Run);
        thread1.Start();
    }

    public void markChanged(){
        changed = changed ? false : true;
    }

    public static StressData GetInstance()
    {
        return StressData.instance;
    }

    public double? GetStressGauge()
    {
        return this.stressGauge;
    }
    public long GetLastUpdateTime()
    {
        return this.lastUpdateTime;
    }
    public bool SetLastUpdateTimeIfBigger(long newTime)
    {
        if (newTime > this.lastUpdateTime)
        {
            this.lastUpdateTime = newTime;
            return true;
        }
        return false;
    }
    public void SetStressGauge(double? gauge)
    {
        this.stressGauge = gauge;
    }
    public void Kill()
    {
        StressDataBridge.GetInstance().Kill();
    }

    private double stress_level;
    
    double last_stress_level;
    float last_time_updated_stress_level;
    // public double getStressLevel(){
    //     last_stress_level = stress_level;
    //     if (changed){ // every second update the stress level

    //         markChanged();
    //         last_time_updated_stress_level = Time.realtimeSinceStartup;
    //         last_stress_level = stress_level;
    //         if (stressGauge == null)
    //             return 0.0;
    //         else
    //             stress_level = (double)stressGauge;
    //     }
    //     double delta = Time.realtimeSinceStartup - last_time_updated_stress_level; // is between [0,1]
    //     return (stress_level - last_stress_level) * delta + last_stress_level;
    // }





    public double getStressLevel(){
        if (changed){
            markChanged();
            last_time_updated_stress_level = Time.realtimeSinceStartup;
            Debug.Log("get stress level using dummy block, fix code at stressDataBridge");
            stress_level = 0.0;
        }
        double delta = Time.realtimeSinceStartup - last_time_updated_stress_level; // is between [0,1]
        Debug.Log("delta: " + delta);
        return (GameObject.Find("global_env").GetComponent<Env>().max_stress_level) * (delta * 2.0);

    }

}       
