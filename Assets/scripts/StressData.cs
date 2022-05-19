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

public class StressDataBridge
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

    private static readonly StressData instance = new StressData();        
    private StressData()    
    {    
        this.stressGauge = null;
        this.lastUpdateTime = 0;

        Thread thread1 = new Thread(StressDataBridge.GetInstance().Run);
        thread1.Start();
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
}       