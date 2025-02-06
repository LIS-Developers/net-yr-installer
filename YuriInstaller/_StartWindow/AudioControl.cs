using NAudio.Wave;
using System.Runtime.CompilerServices;
using YuriInstaller.MizukiTools;
using YuriInstaller.Properties;

namespace YuriInstaller
{
    partial class Comment
    {
        /*
            这里管理程序所有的音频。
         */
    }

    partial class _StartWindow
    {
        /// <summary>验证失败声。<br />
        /// Verify failed sound.</summary>
        private static readonly MizukiWaveOutEvent pengci = new MizukiWaveOutEvent(Resources.pengci);

        /// <summary>切换图片声。<br />
        /// Image change sound.</summary>
        private static readonly MizukiWaveOutEvent imageChange = new MizukiWaveOutEvent(Resources.ImageChange);

        /// <summary>背景音乐播放器。<br />
        /// BGM Player.</summary>
        private static MizukiWaveOutEvent backgroundMusic;

        /// <summary>菜单更新音效。<br />
        /// Button moving sound.</summary>
        private static readonly MizukiWaveOutEvent MenuUpdate = new MizukiWaveOutEvent(Resources.MenuUpdate);

        /// <summary>播完了就重播。<br />
        /// Replay when bgm ended.</summary>
        private void BGM_MediaEnded(object sender, StoppedEventArgs e)
        {
            backgroundMusic?.Replay();
        }

        /// <summary>初始化背景音乐播放器。<br />
        /// Initialize BGM player.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InitializeBGM()
        {
            backgroundMusic = new MizukiWaveOutEvent(Resources.BackgroundMusic)
            {
                NotGlobalQuiet = true
            };
            backgroundMusic.PlaybackStopped += BGM_MediaEnded;
            backgroundMusic.Play();
        }
    }
}
