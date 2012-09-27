using System;
using System.Windows;
using Microsoft.Phone.BackgroundAudio;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace AudioPlaybackAgent1
{
    public class AudioPlayer : AudioPlayerAgent
    {
        private static volatile bool _classInitialized;

        /// <remarks>
        /// Экземпляры AudioPlayer могут совместно использовать один и тот же процесс. 
        /// Статические поля могут использоваться для распределения состояния между экземплярами AudioPlayer
        /// или для взаимодействия с агентом потокового аудио.
        /// </remarks>
        private TimeSpan position;
        public AudioPlayer()
        {
            if (!_classInitialized)
            {
                _classInitialized = true;
                // Подпишитесь на обработчик управляемых исключений
                Deployment.Current.Dispatcher.BeginInvoke(delegate
                {
                    Application.Current.UnhandledException += AudioPlayer_UnhandledException;
                });
                //Podcast pc = new Podcast();
                position = TimeSpan.Zero;
            }
        }

        /// Код для выполнения на необработанных исключениях
        private void AudioPlayer_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Произошло необработанное исключение; перейти в отладчик
                System.Diagnostics.Debugger.Break();
            }
        }

        /// <summary>
        /// Вызывается при изменении состояния воспроизведения, за исключением состояния ошибки (см. OnError)
        /// </summary>
        /// <param name="player">BackgroundAudioPlayer</param>
        /// <param name="track">Дорожка, воспроизводимая во время изменения состояния воспроизведения</param>
        /// <param name="playState">Новое состояние воспроизведения проигрывателя</param>
        /// <remarks>
        /// Изменения состояния воспроизведения невозможно отменить. Они вызываются, даже если изменение состояния
        /// было вызвано самим приложением при условии, что в приложении используется обратный вызов.
        ///
        /// Важные события playstate: 
        /// (а) TrackEnded: вызывается, когда в проигрывателе нет текущей дорожки. Агент может задать следующую дорожку.
        /// (б) TrackReady: звуковая дорожка задана и готова для воспроизведения.
        ///
        /// Вызовите NotifyComplete() только один раз после завершения запроса агента, включая асинхронные обратные вызовы.
        /// </remarks>
        protected override void OnPlayStateChanged(BackgroundAudioPlayer player, AudioTrack track, PlayState playState)
        {
            switch (playState)
            {
                case PlayState.TrackEnded:
                    player.Track = GetPreviousTrack();
                    break;
                case PlayState.TrackReady:
                    player.Play();
                    break;
                case PlayState.Shutdown:
                    // TODO: обработайте здесь состояние отключения (например, сохраните состояние)
                    break;
                case PlayState.Unknown:
                    break;
                case PlayState.Stopped:
                    break;
                case PlayState.Paused:
                    //здесь нужно запомнить текущую позицию для восстановления воспроизведения
                    break;
                case PlayState.Playing:
                    break;
                case PlayState.BufferingStarted:
                    break;
                case PlayState.BufferingStopped:
                    break;
                case PlayState.Rewinding:
                    break;
                case PlayState.FastForwarding:
                    break;
            }

            NotifyComplete();
        }


        /// <summary>
        /// Вызывается при запросе пользователем действия с помощью пользовательского интерфейса приложения или системы
        /// </summary>
        /// <param name="player">BackgroundAudioPlayer</param>
        /// <param name="track">Дорожка, воспроизводимая во время действия пользователя</param>
        /// <param name="action">Действие, запрошенное пользователем</param>
        /// <param name="param">Данные, связанные с запрошенным действием.
        /// В текущей версии этот параметр используется только с действием поиска
        /// для обозначения запрошенного положения в звуковой дорожке</param>
        /// <remarks>
        /// Действия пользователя не изменяют автоматически состояние системы; за выполнение действий
        /// пользователя, если они поддерживаются, отвечает агент.
        ///
        /// Вызовите NotifyComplete() только один раз после завершения запроса агента, включая асинхронные обратные вызовы.
        /// </remarks>
        protected override void OnUserAction(BackgroundAudioPlayer player, AudioTrack track, UserAction action, object param)
        {
            switch (action)
            {
                case UserAction.Play:
                    if (player.PlayerState != PlayState.Playing)
                    {
                        List<string> atrack = DeserializeFromIsolatedStorage<List<string>>("podcast.link");
                        
                        //AudioTrack pcast = new
                        Uri track_url = new Uri(atrack[0], UriKind.Absolute);
                        if (player.Track == null)
                        {
                            AudioTrack podcast = new AudioTrack (track_url,
                            atrack[1],
                            atrack[2],
                            atrack[3],
                            null);

                            player.Track = podcast;
                        }
                        else if (player.Track.Source != track_url)
                        {
                            AudioTrack podcast = new AudioTrack(track_url,
                                atrack[1],
                                atrack[2],
                                atrack[3],
                                null);
                            player.Track = podcast;
                        }
                        
                        player.Play();
                        break;
                    }
                    break;
                case UserAction.Stop:
                    player.Stop();
                    break;
                case UserAction.Pause:
                    player.Pause();
                    break;
                case UserAction.FastForward:
                    player.FastForward();
                    break;
                case UserAction.Rewind:
                    player.Rewind();
                    break;
                case UserAction.Seek:
                    player.Position = (TimeSpan)param;
                    break;
                case UserAction.SkipNext:
                    player.Track = GetNextTrack();
                    break;
                case UserAction.SkipPrevious:
                    AudioTrack previousTrack = GetPreviousTrack();
                    if (previousTrack != null)
                    {
                        player.Track = previousTrack;
                    }
                    break;
            }

            NotifyComplete();
        }


        /// <summary>
        /// Реализует логику для получения следующего экземпляра AudioTrack.
        /// В списке воспроизведения источником может быть файл, веб-запрос и т. д.
        /// </summary>
        /// <remarks>
        /// Универсальный код ресурса (URI) AudioTrack определяет источник, которым может быть:
        /// (а) файл в изолированной памяти (относительный URI, представляет путь в изолированной памяти)
        /// (б) URL-адрес HTTP (абсолютный URI)
        /// (в) MediaStreamSource (null)
        /// </remarks>
        /// <returns>экземпляр AudioTrack или значение null, если воспроизведение завершено</returns>
        private AudioTrack GetNextTrack()
        {
            // TODO: добавьте логику для получения следующей звуковой дорожки

            AudioTrack track = null;

            // укажите дорожку

            return track;
        }


        /// <summary>
        /// Реализует логику для получения предыдущего экземпляра AudioTrack.
        /// </summary>
        /// <remarks>
        /// Универсальный код ресурса (URI) AudioTrack определяет источник, которым может быть:
        /// (а) файл в изолированной памяти (относительный URI, представляет путь в изолированной памяти)
        /// (б) URL-адрес HTTP (абсолютный URI)
        /// (в) MediaStreamSource (null)
        /// </remarks>
        /// <returns>экземпляр AudioTrack или значение null, если предыдущая дорожка не разрешена</returns>
        private AudioTrack GetPreviousTrack()
        {
            // TODO: добавьте логику для получения предыдущей звуковой дорожки

            AudioTrack track = null;

            // укажите дорожку

            return track;
        }

        /// <summary>
        /// Вызывается в случае ошибки воспроизведения, например, если звуковая дорожка не загружается правильно
        /// </summary>
        /// <param name="player">BackgroundAudioPlayer</param>
        /// <param name="track">Дорожка, в которой произошла ошибка</param>
        /// <param name="error">Произошедшая ошибка</param>
        /// <param name="isFatal">При значении true воспроизведение дорожки невозможно и будет остановлено</param>
        /// <remarks>
        /// Вызов этого метода во всех случаях не гарантируется. Например, если в фоновом агенте 
        /// произошло необработанное исключение, он не будет вызываться для обработки своих ошибок.
        /// </remarks>
        protected override void OnError(BackgroundAudioPlayer player, AudioTrack track, Exception error, bool isFatal)
        {
            if (isFatal)
            {
                Abort();
            }
            else
            {
                NotifyComplete();
            }

        }

        /// <summary>
        /// Вызывается при отмене запроса агента
        /// </summary>
        /// <remarks>
        /// После отмены запроса агент завершает работу в течение 5 секунд
        /// путем вызова NotifyComplete()/Abort().
        /// </remarks>
        protected override void OnCancel()
        {

        }

        private static T DeserializeFromIsolatedStorage<T>(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return default(T);
            }

            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            using (var stream = store.OpenFile(filename, FileMode.Open))
            using (var reader = XmlReader.Create(stream))
            {
                return (T)new XmlSerializer(typeof(T)).Deserialize(reader);
            }
        }
    }

    public class Podcast 
    {
        public TimeSpan position { get; set; }
        
        public Podcast()
        {
            TimeSpan position = TimeSpan.Zero;
        }
    }
}
