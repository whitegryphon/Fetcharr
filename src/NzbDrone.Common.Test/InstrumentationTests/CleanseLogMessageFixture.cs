using FluentAssertions;
using NUnit.Framework;
using NzbDrone.Common.Instrumentation;

namespace NzbDrone.Common.Test.InstrumentationTests
{
    [TestFixture]
    public class CleanseLogMessageFixture
    {
        // Indexer Urls
        [TestCase(@"https://iptorrents.com/torrents/rss?u=mySecret;tp=mySecret;l5;download")]
        [TestCase(@"http://rss.torrentleech.org/mySecret")]
        [TestCase(@"https://rss24h.torrentleech.org/mySecret")]
        [TestCase(@"http://rss.torrentleech.org/rss/download/12345/01233210/file.name-RLSGRP.torrent")]
        [TestCase(@"https://www.torrentleech.org/rss/download/12345/01233210/file.name-RLSGRP.torrent")]
        [TestCase(@"http://www.bitmetv.org/rss.php?uid=mySecret&passkey=mySecret")]
        [TestCase(@"https://rss.omgwtfnzbs.org/rss-search.php?catid=19,20&user=sonarr&api=mySecret&eng=1")]
        [TestCase(@"https://dognzb.cr/fetch/2b51db35e1912ffc138825a12b9933d2/2b51db35e1910123321025a12b9933d2")]
        [TestCase(@"https://baconbits.org/feeds.php?feed=torrents_tv&user=12345&auth=2b51db35e1910123321025a12b9933d2&passkey=mySecret&authkey=2b51db35e1910123321025a12b9933d2")]
        [TestCase(@"http://127.0.0.1:9117/dl/indexername?jackett_apikey=flwjiefewklfjacketmySecretsdfldskjfsdlk&path=we0re9f0sdfbase64sfdkfjsdlfjk&file=The+Torrent+File+Name.torrent")]
        [TestCase(@"http://nzb.su/getnzb/2b51db35e1912ffc138825a12b9933d2.nzb&i=37292&r=2b51db35e1910123321025a12b9933d2")]
        [TestCase(@"https://horrorcharnel.org/takeloginhorror.php: username=mySecret&password=mySecret&use_sslvalue==&perm_ssl=1&submitme=X&use_ssl=1&returnto=%2F&captchaSelection=1230456")]
        [TestCase(@"https://torrentdb.net/login: _token=2b51db35e1912ffc138825a12b9933d2&username=mySecret&password=mySecret&remember=on")]
        [TestCase(@" var authkey = ""2b51db35e1910123321025a12b9933d2"";")]
        [TestCase(@"https://hd-space.org/index.php?page=login: uid=mySecret&pwd=mySecret")]
        [TestCase(@"https://beyond-hd.me/api/torrents/2b51db35e1912ffc138825a12b9933d2")]
        [TestCase(@"Req: [POST] https://www3.yggtorrent.nz/user/login: id=mySecret&pass=mySecret&ci_csrf_token=2b51db35e1912ffc138825a12b9933d2")]
        [TestCase(@"https://torrentseeds.org/api/torrents/filter?api_token=2b51db35e1912ffc138825a12b9933d2&name=&sortField=created_at&sortDirection=desc&perPage=100&page=1")]
        [TestCase(@"https://beyond-hd.me/torrent/download/the-next-365-days-2022-2160p-nf-web-dl-dual-ddp-51-dovi-hdr-hevc-apex.225146.2b51db35e1912ffc138825a12b9933d2")]
        [TestCase(@"https://anthelion.me/api.php?api_key=2b51db35e1910123321025a12b9933d2&o=json&t=movie&q=&tmdb=&imdb=&cat=&limit=100&offset=0")]
        [TestCase(@"https://avistaz.to/api/v1/jackett/auth: username=mySecret&password=mySecret&pid=mySecret")]

        // Indexer and Download Client Responses

        // avistaz response
        [TestCase(@"""download"":""https://avistaz.to/rss/download/2b51db35e1910123321025a12b9933d2/tb51db35e1910123321025a12b9933d2.torrent"",")]
        [TestCase(@",""info_hash"":""2b51db35e1910123321025a12b9933d2"",")]
        [TestCase(@"""token"":""2b51db35e1910123321025a12b9933d2""")]

        // animebytes response
        [TestCase(@"""Link"":""https://animebytes.tv/torrent/994064/download/tb51db35e1910123321025a12b9933d2"",")]

        // danish bytes response
        [TestCase(@",""rsskey"":""2b51db35e1910123321025a12b9933d2"",")]
        [TestCase(@",""passkey"":""2b51db35e1910123321025a12b9933d2"",")]
        [TestCase(@"{""rsskey"":""2b51db35e1910123321025a12b9933d2""}")]
        [TestCase(@"{""passkey"":""2b51db35e1910123321025a12b9933d2""}")]

        // nzbgeek & usenet response
        [TestCase(@"<guid isPermaLink=""true"">https://api.nzbgeek.info/api?t=details&amp;id=2b51db35e1910123321025a12b9933d2&amp;apikey=2b51db35e1910123321025a12b9933d2</guid>")]

        // UNIT3D Response
        [TestCase(@"""download_link"":""https://blutopia.xyz/torrent/download/114592.2b51db35e1910123321025a12b9933d2"",")]
        [TestCase(@"""download_link"":""https://desitorrents.tv/torrent/download/114592.2b51db35e1910123321025a12b9933d2"",")]

        // NzbGet
        [TestCase(@"{ ""Name"" : ""ControlUsername"", ""Value"" : ""mySecret"" }, { ""Name"" : ""ControlPassword"", ""Value"" : ""mySecret"" }, ")]
        [TestCase(@"{ ""Name"" : ""Server1.Username"", ""Value"" : ""mySecret"" }, { ""Name"" : ""Server1.Password"", ""Value"" : ""mySecret"" }, ")]

        // MTV
        [TestCase(@"<link rel=""alternate"" type=""application/rss+xml"" href=""/feeds.php?feed=torrents_notify_2b51db35e1910123321025a12b9933d2&amp;user=(removed)&amp;auth=(removed)&amp;passkey=(removed)&amp;authkey=(removed) title=""MoreThanTV - P.T.N."" />")]
        [TestCase(@"href=""/torrents.php?action=download&amp;id=(removed)&amp;authkey=(removed)&amp;torrent_pass=2b51db35e1910123321025a12b9933d2"" title=""Download Torrent""")]

        // Sabnzbd
        [TestCase(@"http://127.0.0.1:1234/api/call?vv=1&apikey=mySecret")]
        [TestCase(@"http://127.0.0.1:1234/api/call?vv=1&ma_username=mySecret&ma_password=mySecret")]
        [TestCase(@"""config"":{""newzbin"":{""username"":""mySecret"",""password"":""mySecret""}")]
        [TestCase(@"""nzbxxx"":{""username"":""mySecret"",""apikey"":""mySecret""}")]
        [TestCase(@"""growl"":{""growl_password"":""mySecret"",""growl_server"":""""}")]
        [TestCase(@"""nzbmatrix"":{""username"":""mySecret"",""apikey"":""mySecret""}")]
        [TestCase(@"""misc"":{""username"":""mySecret"",""api_key"":""mySecret"",""password"":""mySecret"",""nzb_key"":""mySecret""}")]
        [TestCase(@"""servers"":[{""username"":""mySecret"",""password"":""mySecret""}]")]
        [TestCase(@"""misc"":{""email_account"":""mySecret"",""email_to"":[],""email_from"":"""",""email_pwd"":""mySecret""}")]

        // uTorrent
        [TestCase(@"http://localhost:9091/gui/?token=wThmph5l0ZXfH-a6WOA4lqiLvyjCP0FpMrMeXmySecret_VXBO11HoKL751MAAAAA&list=1")]
        [TestCase(@",[""boss_key"",0,""mySecret"",{""access"":""Y""}],[""boss_key_salt"",0,""mySecret"",{""access"":""W""}]")]
        [TestCase(@",[""webui.username"",2,""mySecret"",{""access"":""Y""}],[""webui.password"",2,""mySecret"",{""access"":""Y""}]")]
        [TestCase(@",[""webui.uconnect_username"",2,""mySecret"",{""access"":""Y""}],[""webui.uconnect_password"",2,""mySecret"",{""access"":""Y""}]")]
        [TestCase(@",[""proxy.proxy"",2,""mySecret"",{""access"":""Y""}]")]
        [TestCase(@",[""proxy.username"",2,""mySecret"",{""access"":""Y""}],[""proxy.password"",2,""mySecret"",{""access"":""Y""}]")]

        // Deluge
        [TestCase(@",{""download_location"": ""C:\Users\\mySecret mySecret\\Downloads""}")]
        [TestCase(@",{""download_location"": ""/home/mySecret/Downloads""}")]
        [TestCase(@",{""download_location"": ""/Users/mySecret/Downloads""}")]
        [TestCase(@"auth.login(""mySecret"")")]

        // Download Station
        [TestCase(@"webapi/entry.cgi?api=(removed)&version=2&method=login&account=01233210&passwd=mySecret&format=sid&session=DownloadStation")]

        // Announce URLs (passkeys) Magnet & Tracker
        [TestCase(@"magnet_uri"":""magnet:?xt=urn:btih:9pr04sgkillroyimaveql2tyu8xyui&dn=&tr=https%3a%2f%2fxxx.yyy%2f9pr04sg601233210IMAveQL2tyu8xyui%2fannounce""}")]
        [TestCase(@"magnet_uri"":""magnet:?xt=urn:btih:9pr04sgkillroyimaveql2tyu8xyui&dn=&tr=https%3a%2f%2fxxx.yyy%2ftracker.php%2f9pr04sg601233210IMAveQL2tyu8xyui%2fannounce""}")]
        [TestCase(@"magnet_uri"":""magnet:?xt=urn:btih:9pr04sgkillroyimaveql2tyu8xyui&dn=&tr=https%3a%2f%2fxxx.yyy%2fannounce%2f9pr04sg601233210IMAveQL2tyu8xyui""}")]
        [TestCase(@"magnet_uri"":""magnet:?xt=urn:btih:9pr04sgkillroyimaveql2tyu8xyui&dn=&tr=https%3a%2f%2fxxx.yyy%2fannounce.php%3fpasskey%3d9pr04sg601233210IMAveQL2tyu8xyui""}")]
        [TestCase(@"tracker"":""https://xxx.yyy/9pr04sg601233210IMAveQL2tyu8xyui/announce""}")]
        [TestCase(@"tracker"":""https://xxx.yyy/tracker.php/9pr04sg601233210IMAveQL2tyu8xyui/announce""}")]
        [TestCase(@"tracker"":""https://xxx.yyy/announce/9pr04sg601233210IMAveQL2tyu8xyui""}")]
        [TestCase(@"tracker"":""https://xxx.yyy/announce.php?passkey=9pr04sg601233210IMAveQL2tyu8xyui""}")]
        [TestCase(@"tracker"":""http://xxx.yyy/announce.php?passkey=9pr04sg601233210IMAveQL2tyu8xyui"",""info"":""http://xxx.yyy/info?a=b""")]

        // BroadcastheNet
        [TestCase(@"method: ""getTorrents"", ""params"": [ ""mySecret"",")]
        [TestCase(@"getTorrents(""mySecret"", [asdfasdf], 100, 0)")]
        [TestCase(@"""DownloadURL"":""https://broadcasthe.net/torrents.php?action=download&id=123&authkey=mySecret&torrent_pass=mySecret""")]

        // Webhooks - Notifiarr
        [TestCase(@"https://xxx.yyy/api/v1/notification/fetcharr/9pr04sg6-0123-3210-imav-eql2tyu8xyui")]
        [TestCase("https://notifiarr.com/notifier.php: api=1234530f-422f-4aac-b6b3-01233210aaaa&radarr_health_issue_message=Download")]

        // RSS
        [TestCase(@"<atom:link href = ""https://api.nzb.su/api?t=search&amp;extended=1&amp;cat=3030&apikey=mySecret&amp;q=Diggers"" rel=""self"" type=""application/rss+xml"" />")]

        // Applications
        [TestCase(@"""name"":""apiKey"",""value"":""mySecret""")]

        // Internal
        [TestCase(@"[Info] MigrationController: *** Migrating Database=fetcharr-main;Host=postgres14;Username=mySecret;Password=mySecret;Port=5432;Enlist=False ***")]
        [TestCase("/readarr/signalr/messages/negotiate?access_token=1234530f422f4aacb6b301233210aaaa&negotiateVersion=1")]
        [TestCase(@"[Info] MigrationController: *** Migrating Database=fetcharr-main;Host=postgres14;Username=mySecret;Password=mySecret;Port=5432;token=mySecret;Enlist=False&username=mySecret;mypassword=mySecret;mypass=shouldkeep1;test_token=mySecret;password=123%@%_@!#^#@;use_password=mySecret;get_token=shouldkeep2;usetoken=shouldkeep3;passwrd=mySecret;")]
        public void should_clean_message(string message)
        {
            var cleansedMessage = CleanseLogMessage.Cleanse(message);

            cleansedMessage.Should().NotContain("mySecret");
            cleansedMessage.Should().NotContain("123%@%_@!#^#@");
            cleansedMessage.Should().NotContain("01233210");
        }

        [TestCase(@"[Info] MigrationController: *** Migrating Database=radarr-main;Host=postgres14;Username=mySecret;Password=mySecret;Port=5432;token=mySecret;Enlist=False&username=mySecret;mypassword=mySecret;mypass=shouldkeep1;test_token=mySecret;password=123%@%_@!#^#@;use_password=mySecret;get_token=shouldkeep2;usetoken=shouldkeep3;passwrd=mySecret;")]
        public void should_keep_message(string message)
        {
            var cleansedMessage = CleanseLogMessage.Cleanse(message);

            cleansedMessage.Should().NotContain("mySecret");
            cleansedMessage.Should().NotContain("123%@%_@!#^#@");
            cleansedMessage.Should().NotContain("01233210");

            cleansedMessage.Should().Contain("shouldkeep1");
            cleansedMessage.Should().Contain("shouldkeep2");
            cleansedMessage.Should().Contain("shouldkeep3");
        }

        [TestCase(@"Some message (from 32.2.3.5 user agent)")]
        [TestCase(@"Auth-Invalidated ip 32.2.3.5")]
        [TestCase(@"Auth-Success ip 32.2.3.5")]
        [TestCase(@"Auth-Logout ip 32.2.3.5")]
        public void should_clean_ipaddress(string message)
        {
            var cleansedMessage = CleanseLogMessage.Cleanse(message);

            cleansedMessage.Should().NotContain(".2.3.");
        }

        [TestCase(@"Some message (from 10.2.3.2 user agent)")]
        [TestCase(@"Auth-Unauthorized ip 32.2.3.5")]
        [TestCase(@"Auth-Failure ip 32.2.3.5")]
        public void should_not_clean_ipaddress(string message)
        {
            var cleansedMessage = CleanseLogMessage.Cleanse(message);

            cleansedMessage.Should().Be(message);
        }

        [TestCase(@"&useToken=2b51db35e1910123321025a12b9933d2")]
        [TestCase(@"&useToken=2b51db35e1910123321025a12b9933d2")]
        public void should_not_clean_usetoken(string message)
        {
            var cleansedMessage = CleanseLogMessage.Cleanse(message);

            cleansedMessage.Should().Be(message);
        }

        [TestCase(@"https://www.torrentleech.org/torrents/browse/list/imdbID/tt8005374/categories/29,2,26,27,32,44,7,34,35")]
        [TestCase(@"https://torrentapi.org/pubapi_v2.php?get_token=get_token&app_id=Fetcharr")]
        public void should_not_clean_url(string message)
        {
            var cleansedMessage = CleanseLogMessage.Cleanse(message);

            cleansedMessage.Should().Be(message);
        }
    }
}
