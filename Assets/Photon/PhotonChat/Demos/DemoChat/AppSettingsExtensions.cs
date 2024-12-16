<<<<<<< HEAD
﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Exit Games GmbH"/>
// <summary>Demo code for Photon Chat in Unity.</summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------


using Photon.Realtime;


namespace Photon.Chat.Demo
{
    public static class AppSettingsExtensions
    {
        public static ChatAppSettings GetChatSettings(this AppSettings appSettings)
        {
            return new ChatAppSettings
                   {
                       AppIdChat = appSettings.AppIdChat,
                       AppVersion = appSettings.AppVersion,
                       FixedRegion = appSettings.IsBestRegion ? null : appSettings.FixedRegion,
                       NetworkLogging = appSettings.NetworkLogging,
                       Protocol = appSettings.Protocol,
                       EnableProtocolFallback = appSettings.EnableProtocolFallback,
                       Server = appSettings.IsDefaultNameServer ? null : appSettings.Server,
                       Port = (ushort)appSettings.Port,
                       ProxyServer = appSettings.ProxyServer
                       // values not copied from AppSettings class: AuthMode
                       // values not needed from AppSettings class: EnableLobbyStatistics 
                   };
        }
    }
=======
﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Exit Games GmbH"/>
// <summary>Demo code for Photon Chat in Unity.</summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------


using Photon.Realtime;


namespace Photon.Chat.Demo
{
    public static class AppSettingsExtensions
    {
        public static ChatAppSettings GetChatSettings(this AppSettings appSettings)
        {
            return new ChatAppSettings
                   {
                       AppIdChat = appSettings.AppIdChat,
                       AppVersion = appSettings.AppVersion,
                       FixedRegion = appSettings.IsBestRegion ? null : appSettings.FixedRegion,
                       NetworkLogging = appSettings.NetworkLogging,
                       Protocol = appSettings.Protocol,
                       EnableProtocolFallback = appSettings.EnableProtocolFallback,
                       Server = appSettings.IsDefaultNameServer ? null : appSettings.Server,
                       Port = (ushort)appSettings.Port,
                       ProxyServer = appSettings.ProxyServer
                       // values not copied from AppSettings class: AuthMode
                       // values not needed from AppSettings class: EnableLobbyStatistics 
                   };
        }
    }
>>>>>>> 41765e529d69567fde358780eeee7b323ac1420d
}