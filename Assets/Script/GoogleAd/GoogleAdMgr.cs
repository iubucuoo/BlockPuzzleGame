using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using GoogleMobileAds.Common;

public class GoogleAdMgr : MonoSingleton<GoogleAdMgr>
{
    //    广告格式 示例广告单元 ID
    //开屏广告    ca-app-pub-3940256099942544/3419835294
    //横幅广告 ca-app-pub-3940256099942544/6300978111
    //插页式广告 ca-app-pub-3940256099942544/1033173712
    //激励广告 ca-app-pub-3940256099942544/5224354917
    //插页式激励广告 ca-app-pub-3940256099942544/5354046379
    //原生广告 ca-app-pub-3940256099942544/2247696110
    //private RewardedAd rewardedAd;
    // Start is called before the first frame update
    void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        //MobileAds.Initialize(initStatus =>
        //{
        // //   RequestInterstitial();
        //});
        // Initialize the Google Mobile Ads SDK.
        //MobileAds.Initialize(initStatus => {
        //    this.rewardedAd = new RewardedAd(adUnitId);

        //    // Create an empty ad request.
        //    AdRequest request = new AdRequest.Builder().Build();
        //    // Load the rewarded ad with the request.
        //    this.rewardedAd.LoadAd(request);
        //});
        MobileAds.Initialize(HandleInitCompleteAction);
    }
    private void HandleInitCompleteAction(InitializationStatus initstatus)
    {
        // Callbacks from GoogleMobileAds are not guaranteed to be called on
        // main thread.
        //不能保证在主线程上调用来自 GoogleMobileAds 的回调。
        // In this example we use MobileAdsEventExecutor to schedule these calls on
        // the next Update() loop.
        //在本例中，我们使用 MobileAdsEventExecutor 在下一个 Update() 循环中安排这些调用。
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            RequestBannerAd();
            RequestInterstitial();
        });
    }
    private BannerView bannerView;

    public void RequestBannerAd()
    {
        // These ad units are configured to always serve test ads.
        //#if UNITY_EDITOR
        //        string adUnitId = "unused";
#if UNITY_ANDROID
        //string adUnitId = "ca-app-pub-3940256099942544/6300978111";
        string adUnitId = "ca-app-pub-1634842308647830/7755028130";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up banner before reusing
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        // Create a 320x50 banner at top of the screen
        bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);

        // Add Event Handlers
        //bannerView.OnAdLoaded += (sender, args) => OnAdLoadedEvent.Invoke();
        //bannerView.OnAdFailedToLoad += (sender, args) => OnAdFailedToLoadEvent.Invoke();
        //bannerView.OnAdOpening += (sender, args) => OnAdOpeningEvent.Invoke();
        //bannerView.OnAdClosed += (sender, args) => OnAdClosedEvent.Invoke();

        // Load a banner ad
        bannerView.LoadAd(new AdRequest.Builder().Build());
    }


    //插页式广告
    private InterstitialAd interstitial;
    //请求插页式广告
    public void RequestInterstitial()
    {
        //if (interstitial != null)
        //{
        //    Debug.LogError("已经开始加载插页式广告");
        //    return;
        //}
        InterstitialDes();
#if UNITY_ANDROID
        //string adUnitId = "ca-app-pub-3940256099942544/1033173712";
        string adUnitId = "ca-app-pub-1634842308647830/9893645892";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(adUnitId);
        
        //添加自定义行为
        
        //在广告请求加载失败时调用。
        interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        //在显示广告时调用。
        interstitial.OnAdOpening += HandleOnAdOpened; 
        //在成功加载广告请求时调用。
        interstitial.OnAdLoaded += HandleOnAdLoaded;
        //在广告关闭时调用。
        interstitial.OnAdClosed += HandleOnAdClosed;
        // 当广告点击导致用户离开应用程序时调用。
        //this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;


        //加载广告
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        interstitial.LoadAd(request);
    }

    private void HandleOnAdClosed(object sender, EventArgs e)
    {
        //DebugMgr.LogError("广告关闭");
        InterstitialDes();
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            //DebugMgr.LogError("广告关闭下一个update调用");
            CallAdClose();
            AudioMgr.Inst.UnpauseMusic();
            RequestInterstitial();
        });
    }
  
    private void HandleOnAdOpened(object sender, EventArgs e)
    {
        //DebugMgr.LogError("显示广告");
    }

    private void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        DebugMgr.LogError("广告请求加载失败。" + e.LoadAdError.GetMessage());
    }

    private void HandleOnAdLoaded(object sender, EventArgs e)
    {
        //DebugMgr.LogError("成功加载广告请求。");
    }
    void CallAdClose()
    {
        if (SwAdCallBack != null)
        {
            SwAdCallBack();
            SwAdCallBack = null;
        }
    }
    Action SwAdCallBack;
    //展示广告
    public void SWAd(Action cb)
    {
        if (interstitial.IsLoaded())
        {
            //DebugMgr.LogError("展示广告");
            AudioMgr.Inst.PauseMusic();
            interstitial.Show();
            SwAdCallBack = cb;
        }
        else
        {
            DebugMgr.LogError("广告没有加载完成");
            cb.Invoke();
        }
    }
    //清理插页式广告
    //创建完 InterstitialAd 后，请确保在放弃对它的引用前调用 Destroy() 方法。
    void InterstitialDes()
    {
        if (interstitial != null)
        {
            //Debug.LogError("Destroy 插页式广告");
            interstitial.Destroy();
            interstitial = null;
        }
    }
}
