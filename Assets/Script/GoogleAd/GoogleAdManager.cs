using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class GoogleAdManager : MonoBehaviour
{
    public static GoogleAdManager Inst;
    private void Awake()
    {
        Inst = this;
    }
    //    广告格式 示例广告单元 ID
    //开屏广告    ca-app-pub-3940256099942544/3419835294
    //横幅广告 ca-app-pub-3940256099942544/6300978111
    //插页式广告 ca-app-pub-3940256099942544/1033173712
    //激励广告 ca-app-pub-3940256099942544/5224354917
    //插页式激励广告 ca-app-pub-3940256099942544/5354046379
    //原生广告 ca-app-pub-3940256099942544/2247696110



    //public string adUnitId = "ca-app-pub-1634842308647830/9893645892";
    //public string adUnitId = "ca-app-pub-1634842308647830/9893645892";

    //private RewardedAd rewardedAd;
    // Start is called before the first frame update
    void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        //MobileAds.Initialize(initStatus => {
        //    this.rewardedAd = new RewardedAd(adUnitId);

        //    // Create an empty ad request.
        //    AdRequest request = new AdRequest.Builder().Build();
        //    // Load the rewarded ad with the request.
        //    this.rewardedAd.LoadAd(request);
        //});
    }



    //插页式广告
    private InterstitialAd interstitial;
    //请求插页式广告
    public void RequestInterstitial()
    {
        if (interstitial!=null)
        {
            Debug.LogError("已经实例化过");
            return;
        }
#if UNITY_ANDROID
        //string adUnitId = "ca-app-pub-3940256099942544/1033173712";
        string adUnitId = "ca-app-pub-1634842308647830/9893645892";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);
        /*
        //添加自定义行为
        //在成功加载广告请求时调用。
        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        //在广告请求加载失败时调用。
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        //在显示广告时调用。
        this.interstitial.OnAdOpening += HandleOnAdOpened; */
        //在广告关闭时调用。
        this.interstitial.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.当广告点击导致用户离开应用程序时调用。
        //this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;
       

        //加载广告
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }

    private void HandleOnAdClosed(object sender, EventArgs e)
    {
        AudioManager.Inst.UnpauseMusic();
        //Debug.LogError("在广告关闭时调用");
        InterstitialDes();
        CallGameOver();
    }

    private void HandleOnAdOpened(object sender, EventArgs e)
    {
        Debug.LogError("在显示广告时调用");
    }

    private void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        Debug.LogError("在广告请求加载失败时调用。"+ e.LoadAdError.GetMessage());
    }

    private void HandleOnAdLoaded(object sender, EventArgs e)
    {
        Debug.LogError("在成功加载广告请求时调用。");
    }
    void CallGameOver()
    {
        if (GameOverA != null)
        {
            GameOverA();
            GameOverA = null;
        }
    }
    Action GameOverA;
    //展示广告
    public void GameOver(Action cb)
    {
        if (this.interstitial.IsLoaded())
        {
            AudioManager.Inst.PauseMusic();
            this.interstitial.Show();
            GameOverA = cb;
        }
    }
    //清理插页式广告
    //创建完 InterstitialAd 后，请确保在放弃对它的引用前调用 Destroy() 方法。
    void InterstitialDes()
    {
        if (interstitial!=null)
        {
            //Debug.LogError("Destroy 插页式广告");
            interstitial.Destroy();
            interstitial = null;
        }
    }
}
