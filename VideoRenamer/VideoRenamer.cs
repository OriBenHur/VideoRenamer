﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace VideoRenamer
{
    public partial class VideoRenamer : Form
    {
        public VideoRenamer()
        {
            InitializeComponent();
        }

        private void VideoRenamer_Load(object sender, EventArgs e)
        {
            var width = listView.Size.Width - 52;
            listView.View = View.Details;
            listView.FullRowSelect = true;
            listView.Columns.Add("", 24);
            listView.Columns.Add("#", 24);
            listView.Columns.Add("Original Name", width / 2);
            listView.Columns.Add("New Name", width / 2);
            listView.CheckBoxes = true;
        }


        private void SizeLastColumn(ListView listview)
        {
            var width = listView.Size.Width - 52;
            listview.Columns[2].Width = width / 2;
            listview.Columns[3].Width = width / 2;
        }

        private void listView_Resize(object sender, EventArgs e)
        {
            SizeLastColumn((ListView)sender);
        }


        private static IList<string> Filtered_List(IList<string> list)
        {
            List<string> tmp = new List<string>();
            foreach (var name in list)
            {
                var extension = Path.GetExtension(name);
                if (extension == null) continue;
                var ext = extension.ToLower();
                if (ext.Equals(".mp4") || ext.Equals(".avi") || ext.Equals(".mkv") || ext.Equals(".srt"))
                {
                    tmp.Add(name);
                }
            }
            return tmp;
        }

        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView.Items.Clear();
            var i = listView.Items.Count;
            var fs = new FolderSelectDialog();
            var result = fs.ShowDialog();
            if (!result) return;
            if (Directory.Exists(fs.FileName))
            {

                var allfiles = Filtered_List(GetFiles(fs.FileName, "*.*"));
                var groupRegex =
                    new Regex(
                        @"((?i)0Ac|0MNiDVD|0PTiMUS|0SEC|0TV|1080|10Laughing|128mns|12paNDas|1337x|135-!|1440|187HD|1920|1TiME|1TV|21TH|2Brothers|2DVD|2HD|2Lions|2ND|2PaCaVeLi|2SD|2TU|2WIRE|310yuma|3DA|3Li|3LT0N|3LTON|3M4|3Mhome|3MiNA|3rD|3rdmillenium|3To1|41S|41ST|420RipZ|420SHAGGER|4FuN|4HM|4HUN|4IL|4KiSS|4LiBeRtY|4PL|4RiLLa|4SJ|4SP0RT|4SPiTE|4UAlone|4YOU|55L|56k|5come5|64K|666|6POiNT6|7aS|7SiNS|850105|9.GiRL|999|9FisH|9RS|A-Destiny|A-E|A-E.Ureshii|A-Et|A-F|A-Faith|A-K|A-KA|A-Keep_AH|A-KF|A-Kraze|A-L|A-M|A-nim-e|A-O|A-R|A-Ru|a-S|A-Swe|A-T|A-W|A.C.A|a.f.k.|A.MC|A.MC.BKF|A2-Subs|A2000A|A24|A40|a4e|aacrime|aAF|aarinfantasy|aarinfantsy_RyRo|AaS|ABB|aBD|ABH|ABiS|Abjex|AbSurdity|ABU|AC-Subs|AC3HD|ACAB|ACCLAiM|ACED|aceford|ACF|ACFS|ACG|ACK|AckTiv3|ACL|AConan|ACX|ACX-SaintDeath|ADAPT|ADC-Elites|ADC-Germany|ADHD|ADTRW|ADX|AE-Kaen|AEGiS|AEN|AERiAL|AERO|AEROHOLiCS|aF|AF-F|aFever|AFFiNiTY|AFFP|AFG|AFO|AFP|AFS|AFz|AG|aGGr0|AH|AHD|aheintz|Ahodomo|AHQ|AIDA|Aiden0|AiHD|AINEX|AiRLiNE|AiRWAVES|aiTen|AJ|AJA|AJAX|Ajna|AK|AK-Subs|AKA|akf|aki|AKIBA|AKiNA|AKIO|AKU|AKUPX|ALANiS|alE13|ALeSiO|ALLiANCE|ALLZINE|ALTEREGO|AMALGAM|AMBASSADOR|AMBIT|AMBiTiOUS|AME|AMIABLE|AMIRITE|AMSTEL|AmX|AN-Classic|AN0NYM0US|AnaF|ANARCHY|aNBc|ANBU|Anbu-Solar|ANBU.umai|AnCo.2|aNDy|ANE|AnFs|ANGELiC|Ani-Kraze|aNi-PrO|AniCom|ANiHLS|AniKami|AniKat|Animanda|Anime-Ancestors|Anime-Conan|Anime-Koi|Anime-Legion|anime-rg|Anime-Supreme|Anime-Takeover|anime_fin|Animebreaker|AniMecha|AnimeClipse|AnimeKai|AnimeNOW|AnimeRG|AnimesPLUS|AnimeYuki|Anipl0x|ANiPUNK|AniTen|ANiURL|ANiVCD|AniYoshi|ANK|ANON|anoXmous|AnS|ANUBIS|AnY|AnY_ShiFa|aO|aO.PM|AOE|Aoi_Crossfade|Aoi_Wasurenai|AonE|AonE_A-Kingdom|AonE_AnY|AonE_HQA|AoT|AP|APAX|APB|APPEL|APT|AQOS|AR|ARC|Archerr|ARCHiViST|ArenaBG|ARGON|ARIGATOU|ARiGOLD|ARiSCRAPAYSiTES|ARK01|ARR|ARROW|ARTHOUSE|ArtSubs|AS|AS-S|asaadas|ASAP|ASCENDANCE|Asenshi|Asenshi-GJM|AsiSter|ASO|ASOURCE|ASS|ass-h|Ass-Hentai|ASSASSiNS|ASU|AT-X|Atavus|aTerFalleT|ATiEM|Atlas47|ATS|AtsA|ATTENTATET|AUC|AURORA|AuSy|AUTHORiTY|AUTV|AVCDVD|AVCHD|AVENUE|AVF|AVG|AviXMaN|AVS|AVS1080|AVS720|AW|awa|aWake|AWGS|AXE|AXED|AXIAL|AXiNE|AXNPL.N6|aXXo|AYAKO|AYAKO-SAE|Ayako_Himatsubushi|AYMO|AyoEunji|AyoSuzy|AYU|AzNiNVASiAN|AZRAEL|AZU|AZuRRaY|B-A|B-G|B-G_m.3.3.w|B.F|B2E|BA|Ba-Su|BABYLON|babylonad|BaCKToRG|BaCo|BaDTaStE|BAF|BAGS|BaibaKo|BAJSKORV|Baka-Anime|Baka-Chi|Bakakozou|Bakaniichan|Bakura2|BaLD|BALLERINA|BAMBOOCHA|BamHD|BAND1D0S|BANKAi|BANZAI|BaraTV|BARBA|BARC0DE|BARGE|BaSS|Bassline|BATV|BAUM|BAVARiA|BAWLS|Bazinga-Subs|BBDvDR|BBF|BBFiTA|BBT-RMX|BBV|BBZ|bc10|BDClub|BDFLiX|BDiSC|BDP|BDr|BDRETAil|BDS|beAst|BEAVE|BEEF.STEW|BeefStew|BEER|BeFree|BeStDivX|BeStDvD|BestHD|BET|BetaDoctor|BETAMAX|BF1|BFF|BG|BgFr|BHATTI|BiA|BiDA|BiEN|BiERBUiKEN|BiFOS|BIFTIES|BiGBruv|BIGTOPS|Billman424|BiPOLAR|BiQ|BiRDHOUSE|BiTo|BKF|BKT|BladeBDP|BlaZe|BLiN|Blixten|BLooDWeiSeR|BLOW|Blu-bits|BluByte|BluDragon|BlueBird|blueF|BlueFixer|Bluereaper|BlueTV|BluEvo|BLUEYES|blueZilla|BluHD|BluntSlayer-Obfuscated|BluWave|BLUWORLD|BMB|BMF|BNFs|BnS|BoB|BoBo|BOKUTOX|BonG|bonkai77|BoO|BOOBIES|BORDERLiNE|BORGATA|BountyHunters|BOV|BOW|BoX|BRADJE|BRASTEMP|bReAK|BREiN|BrG|BRiGAND|BRiGHT|BRISK|BRMP|BRN|BRUTUS|BRZONE|BS|BSEnc|BSS|BTCHKEK|btfHD|BTN|BTSD|BTSFilms|BTT|BTW|BugZ|BugzBunny|BULLDOZER|BUNNY|BURGER|BurnFre@k.tk-Crew|BUX|BVM|BWB|bySnoper|c-f|C-N|C-N.NTFS|C-N.NTFS.No|C-P-S|C-Subs|c0nFuSed|c0re|C1|C4DVD|C4N4B1S|C4TV|C7B|CA|CACHE|CAELUM|CAF|CAKE|CAMELOT|CAMERA|CAMSTAiN|CAP|CAPH|CarVeR|CasStudio|CATCH|CATCHPHRASE|catflap|CBB|CBFM|CBGB|cbm|CCAT|CCCAM|CCCUNT|CCS-Speed|CDC|CDD|CDDHD|CDevils|CEA|CEB|CENSORED|CENTi|CENTROPY|CF|CFE|cfh|CFS|CH-S|CHAKRA|Chaos-MII|chaostheory|CHARA|charliebartlett|CHaWiN|CHD|CHDBits|CHDSUBS|CHDTV|CHEL|CHGRP|CHIBI|Chica-F|Chihiro|Chihiro-Sprocket|ChiLLuM|Chinurarete|CHIPTEC|Chotab|CHRONiCLES|CHRONO|CHUPPI|CHwDgB|chyuu|CiA|CiC|CiELO|CineBus|CiNEFiLE|CiNEFOX|CINEMANIACS|CinePlexx|CiNEVCD|CiRCLE|CIS|CJ|CLASS|CLASSiC|CLASSiCAL|CLD|CLDD|CLERKS|CLITORI|cloudyvision|cLT|CLUE|CM8|Cman|CME|CMS|CNHD|CNHDA|CnK|cNLDVDR|CnT|cntc|CoalGG|Coalgirls|CoalGuys|COALiTiON|COC|COCAIN|Cocksure|Commie|Commie-UNDERWATER|COMPLIANT|COMPULSION|CONDITION|ConePoneFlicks|CONFiDENT|Connaz-AKA-MrPirate|CONTRiBUTiON|Coo7|CoolGuys|COOLHD|COTTAGE|COUNCiL|COUNTERFEIT|COVEIRO|COWiSO|CoWRY|CP|CPtScene|CPY|CR|CRAVERS|Crazy4ad|CREEDANCE|CREEPSHOW|CREMOSA|CREST|CRF|CRiME|CRiMSON|CRiSC|CRiSP|CriTiCAL|CRNTV|CROOKS|CROSSBOW|CROSSFADE|CROWN|CrunchySubs|Crustyroll|CRYS|CRYSTAL|CSHD|CSS|CT.FS|CTD|Cthuyuu|Ctrl-Znf|CtrlHD|CtrlSD|CTU|CTUSports|CULT|CultHD|CuMBuCKeTS|CureCom|CureCom-Doremi|CURIOSITY|Cutting.EDGE|Cyber12.com|CYBERMEN|CyberTyger|Cyphanix|CyTSuNee|D-A|D-F|D-FS|D-R|D-Subs|D-YFI|D-Z0N3|D.L|D05|D0NK|D0PE|D2V|D3FiL3R|d3g|D3Si|D_Z0N3|DA-C|DAA|DaDuck|DAFTPUNK|Dali-Neko|DAMAN|DameDesuYo|danger2u|DANGO|danirl|Danishbits|DANNY|DanskKulturskat|Darkside.RG|DARKTIGER|DARM|DASH|DATS|DAW|DAWGS|DAWGSSERiER|DB|dbR|DBSuper|DC|DC-AG|DCA|DcN|DCP|DCT|DCTP|DD|DDC|dddc|DDR|de\[42\]|DEA|DeadFish|DEADPiXEL|DEADPOOL|DEAL|DEATHTV|DeBCz|DeBTViD|DECADE|DECENT|decibeL|DeeJayAhmed|DEFACED|DEFiNE|DEFiNiTE|DefinitelyNotMe|DEFiNiTiON|DEFLATE|DEFUSED|DeimonHigh-Marche|DEiMOS|DEiTY|DELiCiOUS|DEMAND|DEPRAViTY|DEPRiVED|DEPTH|DERANGED|DerSchuft|DESIRE|DESiRED|desnsurrender|DESS|DETAiLS|DeTvaVe|DEUTERIUM|DEViANT|DEViSE|DEWSTRR|DFA|DFE|DFQ|DFTU|DGAS|DGX|DGz|dH|DHD|DiAMOND|DIAMONDS|DiCH|DiDaKe|DiDee|DiETER|DiFFERENT|DIFFUSION|DiGiCo|DigiSubs|DIMAPIKS|DIMENSION|DiMiTri|DiMMAN|DiNA|DiR|DiRTY|disc|DiSPOSABLE|DiSSENT|DISTILL|DiSTRiCT|disturbia|DiTa|DIV|DiVAS|DiVER|DiVERGE|DiVERSiFY|DiVERSiTY|DiViN3|DiViSiON|DivXNL|DivXNL-Team|DjRobo38|DK|DKiDS|DLTVR|dmd|DMT|DNA|DnB|DNL|DNR|dns|DoA|DOCUMENT|DOESNTSUCK|DOGE|Doitsu|Doki|DOLLHEAD|dominion|DOMiNO|DON|DONATELLO|DonC|DoNE|DoR2|DOREMI|Doremi-Asenshi|DOT|DOUBT|DOUCEMENT|Dowcker|DOWN|DP|dpf|DPiMP|DRABBiTS|DRACULA|DragonTeam|DREAM|DREAMINGROSES|DREAMLiGHT|DRG|DRHD|DROiDS|DRONES|DrSn|DS|DS9|DsM4U|DsunS|Dtech|DTFS|Dublado|DUKES|DUMBLE|DUMMY|DUPLI|DUQA|DURHAM|DutchReleaseTeam|DuX|DVD4LiFE|DVDHiTS|DVDMANiA|DvF|DVL|dvsky|DX|DXO|DYNAMiCS|DYNOSAUR|E-D|E-Portal|E.Rev|E7|EA|EAF|EAGLE|EBi|EbP|Ecchi-Subs|ECHiZEN|ECHOS|ECI|eclipse|eclipse-m.3.3.w|ecliptic-BSS|ED|ED-Subs|eDi|eeye|EFA|EFTERLYST|EHLE|EightBit|EIGO|EiMi|Eiri|Ekolb|Electri4ka|ELECTRiC|Electrichka|ELiA|elizabethtga|ELYSIUM|EM0C0RE|EmC|EMERALD|EMLHDTeam|EMPiREHD|EnA|Encoderflaccid|ENCOUNTERS|ENCRyPTED|EnDoR|EnSu|ENT|eots|EP1C|EPHEMERiD|EPiC|EPiK|EPiSODE|EPP|EPSiLON|EPZ|EROBEAT|ERyX|eSc|eSH4Re|ESiR|ESPiSE|ESPN|EsQ|ESSENTiAL|ETACH|ETHOS|ETM|ETMID|ETRG|EU|EucHD|EuchHD|euHD|EuReKA|EUROPA|EUSTASS|EVE|EveTaku|Eveyuu|eViLsCi|EVO|eVoD|EVOLVE|EwDp|EXCELLENCE|eXD|ExDR|EXiLE|Exiled-Destiny|EXIST3NC3|EXPS|EXQUiSiTE|eXtaCY|ExtraTorrentRG|EXTREME|EXViD|EXViDiNT|eztv|F-B|F0rArCHivE|F1|F1nd|F2F|F2L|FA|FABULOUS|FAIRPLAY|FAiRYTALE|FAKKU|FAMiLYGuY|FAN|FANDANGO|FaNSuB|FANTA|FAPCAVE|FARGIRENIS|FASM|FASTHD|FATAL1TY|Fate-Force|FBO|FCC|FEATURE|Feel-Free|FELONY|FESTiS|FETiSH|FEVER|fez|FF|FFF|FFFpeeps|FFNDVD|FFS|FGT|FHD|FHM|FHs|FiCeLLo|FiCO|FiCODVDR|FiddleGoose|FiDELiO|FiDO|FIG|FiHTV|FilmHD|Filmikz|Final8|FiNaLe|FINALLY|FiTTY|FiXi0N|FIXIT|FiXTv|fjall|FKKHD|FKKTV|FLAiR|FLAiTE|FLAKET|FLAME|fLAMEhd|FLATLiNE|FLAWL3SS|FLEET|FLHD|FLiCKSiCK|FliK|Flomp-Rumbel|FLOSER|FLS|FLUBEL|FLX|FMDAB|FmE|FoA|FoAC|FOCUS|FooKas|FOOTBALL|FORCE|ForceBleue|FORGE|FoRM|FORMULA|FORWARD|FourGHD|FoV|FOXY|FPG|FQM|FR34KTV|FRAGMENT|FraMeSToR|FREAKS|FREEDOM|FREHD|FRENCH-JPA|FRiAR|FRiES|FRiESSPORT|FRIGGHD|FROH-BITE|FROST|Frostii|FROSTII-DCTP|Frostii-Saizen|Frostii_Live-eviL|Froth-Bite|FroZen|FSiHD|Ft4U|FTC|FTO|FTP|FTS|FTVDT|FTW-FM|FTW-HD|fty|FUA|FUBUKI|FUCT|FULLHD|FULLSIZE|FUM|FuN|FUNKKY|FUNNER|Funnuraba|FURZI|FUSiON|FUSSOIR|FUtV|FuzerHD|FXG|FxM|FyHD|FZERO|G.AX|g0tm1Lk|G3LHD|G3N3|G_P|GABE|GAGE|GALACTiCA|GANOOL|GAS|GASM|GASM.GAX|gATP|GAX|GAX.NTFS|GAYGAY|Gazdi|GB|gcd|GDR|Ge-Ba|GECKOS|GEHENNA|GeNa|GENESIDE|GENESIS|Genesis-RG|GENJO|GENUiNE|GEO|GERUDO|Getbackers|GFE|GFE.AP|GFS|GFTP|GFTP.GFS|gFViD|GFW|GFY|gg|GHOULS|GHS|GiGAS|GiMCHi|GiNJi|ginseng|gizzmo|GJM|GK|GK.GAX|GK.RTFS|GK.Shayo|GK.subs4u|GL|GLASSES|gleam|GLF|GLY|GM4F|GME|GME2000|GMoRK|GNARLY|GNF|GoApe|GOATLOVE|Goblin10|GoDSMaCK|GoenWae|Gogeta|GokU61|GoLDSToNE|GOOGLE|GooN|GoPanda|GOPI.SAHI|GOPO|GORE|GOREHOUNDS|GOTHiC|Gothicmaster|GoTV|GotWoot|GP-Subs|GR0GG|Graveurexterne|greenbud1969|GREiD|GREVO|GREYHOUNDS|GreyWolf|GrimRipper|GRiMRPR|GriOTS|GRMV|GROND|GRYM|GS|gSS|GSZ|Gtic|GTVG|GUACAMOLE|gudhak|GUNSubs|GUPDVDR|GUR|GVD|GW|GWP|GWViD|GxP|GZCrew|GZP|h-b|H-Chu|H-S|H-S&McS|H2|h264iRMU|H3LL2P4Y|H3XDVD|H72|H@M|HaB|HabanaSCR|HAC|HACO|Hadena|HAFVCD|HAGGiS|HAiDEAF|Hajike.HnG|HALCYON|Hammer71|HANAKO|HaNc0cK|HANDJOB|HANGOVER|HANGPUNG|HANNIBAL|HANrel|HARMY|HarrHD|HatchetGear|Haterman|Hatsuyuki|Hatsuyuki-Kaitou|Hatsuyuki-Suspeedia|Hatsuyuki-Tsuki|Hauu|Hauu~|HAY|HAYAKU|HBHD|HCA|HD-UNiT3D|HD1080|HD4U|HDATV|HDBRiSe|HDBT|HDC|HDChina|HDCLASSiCS|HDClub|HDCP|HDDEViLS|HDEncX|HDEX|HDFiRE|HDFL|HDi|HDL|HDLiTE|HDMaNiAcS|HDME|HDMI|HDmonSK|HDPLANET|HDQ|HDRoad|HDS|HDU|HDVD|HDViSiON|HDW|HDWinG|HDX|HDxT|HEAT|HECTOR|HEH|HEJRA|HELIX|HELLRAZ0R|Hentai-Legacy|Herakler|HEroAP|HEVCguy|HFS|HH|HHH|HHI|Hi-no-Ka|HIDD3N|HiDeF|HiDt|Hien|HiFi|HighCode|HiGHTiMES|Higure_Rumbel|Himatsubushi|Himi|Himi.No|Himitsu|Himitsu_OOM|HIMMEL|Hinshitsu|HiNT|Hiryuu|HiSD|Hitode|Hitode_AnimeSS|Hive-CM8|Hivrolta|Hiyono|HJS|HL|HLS|HMN|HnK|HNR|HNRDVD|HoH|HOLDEM|Hollow-Subs|holz|Hon3y|Hon3yHD|HonE|Honto|HoodBag|HooKah|HORiZON|HORiZON-ArtSubs|HorribleSubs|hoschi|HOSTiLE|HOWL|HP|HP4F|HqDTS|HQE|HQM|HR|hrs|HS|HuBBaTiX|HUBRiS|HuNTER|HuSh|Huzzah|Huzzah.Scum-Scans|Huzzah_Doremi|hV|HWD|HWE|Hyb|HYBRiD|HYBRiS|HyDe|HYDROGEN|HYFS|HYPE|HZ|I-F|I-Z|i7|iaK|iBEX|iCANDY|ICE|iCEHD|iD|IDE|Idi0T|iDiB|idtv|iFH|iFN|iFPD|iFT|iGNHD|iGNiTE|iGNiTiON|IGUANA|iHATE|IHQDR|iHT|IIF|iKA|ILA|iLG|iLGS|iLL|iLLUSiON|iLS|imacRuel1|iMAGiNE|iMBT|iMCARE|IMDTHS|ImE|IMF|IMMERSE|iMMORTALs|imNaKeD|iMPERiUM|IMPOSTERS|iMSORNY|IMUR88|INaM|iNBEV|iNCEPTiON|iNCiTE|iNCLUSION|iNCOGNiTO|iND|iNFAMOUS|INFERNO|iNfInItE_424|iNFOTv|iNGOT|iNjECT|iNjECTiON|iNK|Inko|INNOCENCE|INP|InSaNiTy|iNSECTS|iNSOMNi|iNSPiRED|InSubs|iNT-TD|IntelliQ|iNTENTiON|iNTERNAL|iNTiMiD|INtL|iNVADERS|iNVANDRAREN|iON|iRB|iRoNiCs|iSD|iSG|ISHIN|iSRAELiTE|israntxa|iSUBS|iT00NZ|iTA|itg|iTOONZ|iTSDVD|ITZ|IY-A_Excal|JACKAL|jackoneill|JACKVID|JADE|JaJunge|Jan55|JaneDoe|JAPHSON|Jarzka|Jason28|Jav-Fans|JAVLiU|JBS|JCH|jedi|JENC|JETSET|JFKDVD|JFKXVID|JIVE|JJH|JKR|JMT|JN|Joggie|JohnGalt|JOLLY|JoLLyRoGeR|joseole99|joseole99-TCL|Jozzep|JS|jth|jTV|Juggalotus|JUGGS|JUMANJi|JunkyCez|Junoon|JUPiLER|Just4fun|JYK|JZ|K-F|K-Fans_AF-F|K-FS|k2|kA-Oz|KAA|KAFFEREP|KAGA|KaKa|KALAVALE|KAMERA|Kametsu|Kamigami|Kanarianime|KART3LDVD|KAS|KATA|KaTe|KAZAN|KBEC|kE|KEBABSAX|KEG|KEKKAI|keltz|Kemo-Subs|KESH|kewenyu|KFC|KG|kgl|KH|Khmer-Hentai|KHPP|kHzViD|KickFoot|KICKOFF|KiLLARMAN|KILLERS|KiLT|KINeMA|KINGDOM|KiNGS|KingStoner|KiNOWELT|Kira|Kira-Fansub|KIRA-H|kirklestat|KISA|KiSS|Kizuna|Kizuna.FA|KJKGlobal|KkS|KLASSiGER|KLASSiGERHD|KLAUWAARDS|KLAXXON|KLL|KlockreN|KN3|knifesharp|KNIGHTY1973|KnK|KnKF-Doremi|KNOC|KnU-FS|KOC|KoD|KOENiG|Koffe|Koharubi|KOI|KOLE|KonzillaRG|KooKoo|KOR|KOTOMI|KOTOMI_GMB_F-B|KPR|KRaLiMaRKo|Krispy|Krissz|Kristallprinz|KS|KSH|KSHOWNOW|KSi|KSN|KSN-AEN|ksya|KTR|KubuSubs|Kurai-Subs|KURAMA|Kuro-Hana|KUTH|KyF|KYR|kyussone|Kyuu|Kyuubi|L-E|L-E_AonE|L-F|L-S|L3Subs|LACHUH|LadyN|Lag-Taka|LAJ|LAKO|LamB|LaMe|LameHD|Lamonae|LAP|LARCENY|LAZERS|Lazy-Subs|LCD|LCHD|leetay|LeetHD|LeetXD|LEGEND|LEGi0N|LeON|LeRalouf|LEVERAGE|LEViTY|LF|LFF|LGLuX|LiBERTY|LIBRARIANS|LiebeHD|Lightmaker|lilwoodenboy|LIME|LIMO|LiNE|LiON|LionsDen|LiPAN|LiTE|LIVE-EVIL|LiViDiTY|LizardGods|LKRG|LMAO|LMG|LNFS|LoB|LoD|LoKI|LOL|LOLCATS|LoLHD|LoLi|Loli-pop.Subs|LoneWolf|LooKMaNe|LordVako|LOST|LoT|LoTV|LOUNGE|lOVE|LOVESICK|LP|LPD|LQTV|lrc|LRH|Ls-S|LS-SUBS|LTN|LTRG|LTT|LTU|LU3UR|Lulusubs|Lum1x|LUNAR|Lunarian|LupinTheNerd|LUSO|LUST|LX-Sub|LYCAN|M-AF|M-F|M-L|M-S|m.3.3.w|M.O|M2|M33P|m4u|M4XD0ME|M4XDOME|M794|MACHD|macro|madeec|MAGiC|MAGiCAL|MAGiCViBE|MAHOU|MAiN|MainEvent|MAJESTiC|Makai|Male|Mamiko-Chihiro|Manatsu|MANGACiTY|MANHOLE|Marhaba|MARiNES|marioBombo|MarkerB|MARS|MaRVeLouS|MaxHD|MAXiMUM|MAXPRO|MAXSPEED|MAYHEM|Mayu-Genjo|maz|Mazui|MB|MC|McCain|McFly|MCH|MCHD|MCR|MDB|MDW-Subs|MEAW|MEAZONE|med|MEDDY|MEDiAMANiACS|MEDiEVAL|MEDIUS|MEG|MEGAMACY|MeGusta|MeiohSubs|MELBA|MELiTE|MEMORiES|MenaceIISociety|Mental.RG|MEOW|MeTH|METiS|MF|MFS|MG|MGMatriX|MGS|MHQ|MiGHTY|Mikoto|MiLLENiUM|MiND|MiNDTHEGAP|MiNT|MiRAGETV|MiRAMAX|MIS|MiSFiTS|Mish|MissRipZ|Miyu|Miyuki-Fansubs|MkvCage|MLW|MMI|MoA|MOBBit|MOBEE1|MOCHI|MoE|Moe.AP|MoE_AP|MOEBIUS|MoeTV|MoF|MoH|Mokyu~|MOMENTUM|MOMO|MONK|monkee|Monokage|MOOVEE|Morai|MoreHD|MORiCH|MORiTZ|MOROSE|MOTION|MOTU|MoTv|MOViERUSH|MOViESTARS|Mp4Ba|MPTDVD|Mr. KickASS|MRC|MrLore|MRN|mRs|MRShanku|MS|mSD|MSE|MSNTHRP|MsR|MTB|MTeam|mthodmn101|MUC|MULTiPLY|MULVAcoded|MURDER|MUSiX|MuskeTeerS|MuSt|MUxHD|mV4U|MVGroup|MVM|mVmHD|MVN|MVP|MVS|MW|MWF|MWND|MWS|MXB|MXMG|MyBadAssScene|MYD|MySiLU|MZABI|Mô-Subs|N!K|N-F|N-Subs|N0L|N0TSC3N3|n3xT|NAFSG|NAHOM|Naisu|Nakama-Fansubs|NanaOne|Nanashi|nanku|NANO|napisyPL|NaRB|Narcolepsy|Narutoverse|NaSaMo|NaSu|NATV|NBS|NCmt|NCPX|NcS|NcS-H|NDRT|NeDiVx|NEECHAN|NekoMimi|NekoS|NemDiggers|NEPTUNE|NERDHD|NERDS|NeRoZ|NES|NEUTRINO|NEW.SOURCE|NewArtRiot|NewMov|NewSubs|NeWTeaM|NEZU|NFHD|NGB|NGE|NgN|NGP|NGR|NGSerier|NGXHD|NhaNc3|NHD|NHH|NiBURU|nick64|NicoNico|NicoNicoDaoga|NiF|NightHawk|Nightspeed|niizk|NikonXP|niku|NILE|Nimu|NinjaPanda|nItRo|NiviSubs|NiX|NiXX|NKS|NL.Subs|NLC|NLSGDVDr|NnH|No.TK.BKF|NO1KNOWS|NODLABS|NoGood|NOGRP|NoHaTe|NOHD|NOiR|NoneStop|noobkg|NoobSubs|NORARS|NORDIC.EDITION|NORDIC.RELEASE|NORDiCHD|NorTV|NoSCR|NOSCREENS|NOsegmenT|NOTV|NotWCP|noushi|NoVA|NOVO|NOWiNHD|nox|NPC|NPW|NSN|NSUBS|NT|NTb|NTF|NTFS|NTFS.AP|NTFS.subs4u|NTX|NuMy|NUTBLADDER|NUXX|NVA|NVEE|NWB|NWO|NwS|NWTC|NY2|NYA|NYDIC|Nyoro~n.Subs|NYS|NyTT|Nyu|O-L|O-S|O.K-Subs|O4L|OAS|OBjECT|OBLiGATED|Occor|OCTi|ODOP|OEM|OEM1080|OFS|OhShit|ohys|Okashii|Okashii-RyRo|old-tejo|OLDDAWGS|OldsMan|OMA|OMER|OMERTA|OMGtv|OMiCRON|Omifast|OMP|OmU|ONDEED|Ongaku|ONIGIRI|ONS|ONYX|oO|OOKAMI|OOM|Oosh|OPiUM|OPT!V!D|OPTiC|optimax|ORC|ORDER|ORENJi|ORGANiC|ORiGEN|ORIGIN|ORPHEUS|OS|OSiRiS|OSiTV|Osuald|OTT|OTV|OUTDATED|OUZO|OVERTiME|OYHD|OZC|OZC-EZ8|p-a|P-S|P-Z.SUBS|P0W4|P0W4DVD|P4DGE|Pa@Ph|PADDO|Pankhabd|panos|Pantsu|PANTY|PAPAi|papi|PAROVOZ|PARTICLE|PartyBoy|PATE|PaTHe|PAYiSO|PaYxXx|pbw|PC|PCH|PCS|pcsyndicate|PDSG-Sentosha|PeeWee|PeKI|PELLUCiD|pem|Penumbra|PerfectionHD|PFa|phase|Philanthropy_Sekai|PHOBOS|PhoenixRG|PHoQUE|PhPh|Phr0stY|PiA|Pianex|piepHD|PiKACHU|PiLAF|PiMP|Pimp4003|PiMS|Pineapple_Salad|PiNER|PiNKPANTERS|PIONEER|PiPicK|PiX|PixelHD|PKF|PL|PLANET3|PLASTKASSE|PLAYNOW|playSD|playXD|PLEADERS|PLT|PlutO|PlX|PM|pmHD|PMSVCD|PO2|POCHER|POD|POE|POKE|POKERUS|POLAR|POLISHED|PoOlLa|PoRNDoCtOR|PORNOHOLiCS|PorphyriA|POSITIVE|PosTX|PoT|PotentPortables|PoTuS|PP|PPQ|pradanada|PRECiOUS|PRETAiL|PRETENTIOUS|prevail|PRIME|PriMeHD|PRiNCE|PRIPPS|prithwi|PrivateHD|PRiViLEGED|PROD|PRoDJi|PROGRESS|Project.Kitteh|PROMiSE|PROPHETS|ProPL|PROXY|PRXHD|PS|PS3-TEAM|PSA|PSEUDO|PSH|PSiG|PsiX|PsO|PSV|PSYCHD|PSYPHER|PTBR|Pti|PtP|PTpOWeR|PtS|PTTeam|PublicHD|PUDDING|PUKKA|PUNCH|Purana|PURE|Pure-Anime.biz|Puri|PusherCrew|PuyaSubs|PUZZLE|PVR|PWE|pwq|PxHD|PZE|Q-R|Q0S|QanT|QCF|QDP|qIIq|QiX|Qman|qnr|QoM|QRC|QRUS|QSP|Quali.SlaYer|QuebecRules|QUEENS|quietE|QXE|R&C|R-B|R-F|R0CKED|R10|R2D2|R3QU3ST|R4F|R_Knorloading|Ra-C|Rabbit-Force|rabomil|RaDiuS|RAGEDVD|RAiNCOAT|RAiNDEER|Raizel|RAKUDA|RANDi|RAP|RAPiDCOWS|RARBG|Rare.Share|Rasengan|RAWKiNS|RaX|Razor1911|RB58|RBB|RBT|RC|RCDiVX|RCP|RCVR|RDK123|RDVAS|REACTOR|Reaperza|REAVERS|REBELS|RedBlade|RedFox109e|REDJOHN|ReDone|RedRay|RedXXL|Redµx|REFiNED|RegeLRechT|REGEXP|REGRET|REiGN|REiSERADiO|REKoDE|RELEASE|Release.Lounge|RELEASED|ReLeNTLesS|RELOADED|REMARKABLE|REMAX|REMUX|Repivx|REPLICA|REPRiSE|REPTiLE|REPUBLIC|REPULSiON|RESiSTANCE|RETREAT|RETRO|Rets|rev|REVEiLLE|REViSiON|REVO|REVOLTE|RevQuest.Katana|REVTEAM|REWARD|RFSD|RFtA|rG|RHD|RHooD|RHyTM|RiFF|RIG|RightSiZE|RIKU|RiP|RIPinpieces|RIPLEY|RiplleyHD|Ripp3rX|RippingGods|RiPRG|RiPTATORz|Rising.Sun|RisingSun|RiTALiN|RiTALiX|RiVER|RKcTV|rmhf|RMT|RMTeam|RMx|RnC|RoCK&BlueLadyRG|RoCKRioT|RoFL|ROGER|ROOR|RORI|ROTATE|ROUGH|ROVERS|Royskatt|Rpyleoh|RRH|RS|RS-F|RsF|RSG|RTA|Rtl2x|RTN|RUBLU|RUBY|RuDE|RUDOS|RUELL|RUELL-Raws|RUELL-Subs|RUFGT|RULLSTOL|RUMBEL|Rumbel_sMi|Rumi|RUNNER|RURI|Ruri-Saizen|RUSTED|RUSTLE|RWD|RWP|Rx|Ryotox|RyS|Ryugan|Ryuumaru|S-A|s-f|S-G|S-K|S-Less|S-S|S0LD13R|S0NA|S26|S4A|s4u|S8|S8RHiNO|SA89|SADPANDA|SAGE|SAiMORNY|SaintDeath|SAiNTS|Saitei|SAiVERT|Saizen|Saizen-HnG|SakuraCircle|SallySubs|SAMFD|SamuSubs|sant1|SANTI|SAPHiRE|SAPPHIRE|SARS|SaSHiMi|SaSK0|SATANiC|SATIVA|SAVANNAH|SB-Subs|SbR|SCARED|SCD|Schitbusters|SChiZO|Scratch404|SCRATCHED|SCREAM|ScWb|SD-Project|SD_Taka|SDGiRL|SDH|SDI|SecretMyth|SECTOR7|SeeHD|SEIAKUSETSU|SEiGHT|SEMTEX|SENATiON|SEPTiC|SEQUA|SERIOUSLY|SERUM|SEV|SEVcD|SEVENTWENTY|SEXORS|SexSh0p|seyu|SF|SFM|SFP|SFS|SFS.subs4u|SFT|SFW|SFW-Chihiro|SFWhine|SG|SGKK|ShAaNiG|SHADOW|Shadowman|SHAMNBOYZ|ShaqSalazar|ShareReactor|SharpHD|sharpysword|Shayo|Shayo.RS-F|SHD|SHDXXX|SHFS|Shi-Fa|SHiNiGAMi|SHINJI|Shinji-Nekomimi|Shinsen-Subs|SHINSEN.SUBS|shinythings|shiro|SHiTHEADS|SHiTONLYGERMAN|SHiTSoNy|Shitsu-Subs|SHiTTy|SHO|Shoku-dan|SHORTBREHD|SHS|SHUNPO|SiAT|SiBV|SiC|Sick-Fansubs|sickboy88|SiGHTHD|SiHD|SiLU|Silver.RG|SiMbA|SiMPLE|SimplyReleaseS|SINISTER|SiNNERS|Sir.Paul|SiRiUs.sHaRe|SiSO|SiTV|SKA|SKALiWAGZ|Skazhutin|SKGTV|SkipTowne|SKiTFiSKE|SKYLIGHT|SkyLord|SKYRG|SL|SLHD|SLiGHT|SLikt|SLIVE|SLM|SLO|SLOMO|SLOW|SlowHD|SM-Subs|SM_LoC|SMC|sMi|SMoKeR|SMOKEY|SMRKings|SMRPRTY|Smurfenlars|snavE|SNCo|SNEAK|SNEAkY|SnF|Sno|SNOW|SNS|SNUGGLER|SoCkS|SOF|Softfeng|SoKa|SOLAR|Solar_Faith|SOLDATS|SomeTV|SONiDO|SONS|SOV|SoW|SoXu|SOY|SPABLAUW|SpaceHD|spamTV|SPAREL|SPARKS|SPAROOD|SPEED|SpeedStar|sPHD|Sphinctone1|SPiRO|SPiROTV|SPK|SPLiTSViLLE|SPOOKS|SPOOKY|SPORK|SPORTSBAR|SpotlandRules|SPRiNTER|SpS|SQsR|SQUiZZiEs|sriz|SRN|SRS|sRu|SS|SS-Eclipse|SSF|SSK|SSL|SSP-Corp|St-s|STAGEMAN|STARLIGHT|STARS|StarWars|Stealthmaster|Steins;Sub|STELLA|Stickya|stieg|Stitch_Encodes|STOCK|stoffinho17|STORM|STRANDED|STRATOS|Strawhat|streetwars|STRONG|StyleZz|StyLishSaLH|SU|sub-R|SubDESU|SubDESU-H|Subject16|SUBLiME|SUBMERGE|SuBoXoNe|subs4u|subs4u.GK|subs4u.KAS|subs4u.Kizuna|subs4u.Lightmaker|subs4u.Vidom|subs4u.YKS.No|SubSmith|SUBSTANCE|SUBTiTLES|Subtox|SUBZERO|sUN.sujaidr|SUNSPOT|SUPERiER|SuperNova|SuPReME|SURFER|SuS|Suspeedia|SVA|SVD|SVENNE|SWAGGERHD|sweHD|swePD|Swesub|SWOLLED|SYNDICATE|SYS|SZN_I-O|T-N|T-S|T-Subs|t00ng0d|T0nK4|T33V33|T3RR0R1STS|TACO|Tadashi|Taiyaki Subs|Taiyaki-Subs|Taiyo|Taka|Taka-THORA|Takoyaki|TangoAlpha|tantrum|TAPpika|TARGET|Tass|TASTE|TASTETV|TastyMelon|Taurine|TAXES|TayTO|TB|TBB|TBF|TBP|TBS|TC.Samurai|TCL|TCM|TCPA|TDF|tdl|TDR|TEAM-KAI|TeamRV|TeamTelly|TECH|TeGijA|TEKATE|TELEFLiX|TELEFUNKEN|TELEViSiON|TENEIGHTY|TenkSu|TeNNReeD|Tensai-Anime|Tenzinn|TERMi|TERRA|terribleHD|terribleSD|TESDROX|TFE|tff|TFG|TFiN|TFS|TG7|TGP|The3DTeam|TheBatman|TheCure|TheNewSquad|THENiGHTMARE|THENiGHTMAREiNHD|THEORY|TheRoad|TheWretched|THICK|Thizz|THOR|THORA|THUGLiNE|THUNDER|tib|TiCAL|TiDE|Tigole|TiiX|TilMigSelv|TimeLesSub|TiMELORDS|TiMPE|TiMTY|TinyE|Tipota|TiTANS|TjHD|TK|TK.STORM|TKO|TLA|tlacatlc6|TLF|TM|TMD|TMSF|TN|TNAN|tNe|TnF|TnH|ToF|Toki-D|TOKUS|TOMA|tomcat12|TOPAZ|TOPCAT|TOPHD|TOPPESTOFKEKS|Tormaid|TorrenTGui|TosKMV|Towelie|ToY-RC|tpz|TREBLE|trentalent|TRexHD|TRG|TRIAD|TRiAL|TRIBAL|TRiMEDIA|TRiPS|TROJAN|TrollHD|TrollUHD|trosa|TRSH|TrTd_TeaM|tRuAVC|TruCK|tRuE|TRUEDEF|TRUEFRENCH|tRuEHD|trulle|TS|TSFnF|TSGroup|TsH|tsn|TSP|TSs|TSUKI|tsukikaze|TSUMIKI|TsuSubs|Tsuyoi|TU|TURBO|TURKiSO|TUSAHD|Tushar|TV2LAX9|TV4A|TVA|TVBYEN|TvD|TVFi|TViLLAGE|TvNORGE|TVP|TvR|TVS|TVSmash|TvSTUE|TvTiME|TVTUPA|TW|TWA|TweakAnime|TWEET|twentyforty|TWiST|TWISTER|TWiZTED|TwoPhat|TX|TxN|UAV|UBiK|UbM|UBR|UDC4ALL|uffowich|UFW|UKDHD|UKTV|UKVcd|ULF|ULSHD|ULTiMATE|umai|umee|UMF|UNDEAD|UNDERCOVER|UNDERWATER|Underwater-Mahjong|UNDINE|UNiQUE|UNiT|UNiTaiL|UNiTY|UNKOWN|UNSKiLLED|UNTOUCHABLES|UNTOUCHED|UNVEiL|UPPERCUT|ureshii|Ureshii-Yoroshiku|USELESS|USURY|UTOPiA|UTR|UTR-HD|UTT|UTW|UTW-Mazui|UTW-Thora|UTW-TMD|UTW-Vivid|UTWoots|UVall|UxO|Uyirvani|V-A|VaAr3|VAiN|VALHALLA|VALiOMEDiA|VALKYRiA|VALUE|VAMPS|Vanillapunk|VanRay|VCDVaULT|VCF|VCORE|VDE-Subs|VeDeTT|VeGaN|Vegapunk|VeggTeppe|VENUE|Veritas|VeroVenlo|VERSATiLE|VERTIGGO|VETO|Vex|VhV|VIAHD|VIAZAC|ViCiOsO|Victorique|ViD|VideoCD|VideoStar|vidom|Vidom.FA|VietHD|ViGi|ViKAT|ViLD|ViLLiANS|ViNYL|ViP3R|ViSiON|Vision-Anime|ViSTA|ViSTA™|ViSUM|ViTE|ViVi|ViVID|Vivid-Asenshi|Vivid-Taku|Vivid-Watashi|VLiS|VOA|VOID|VOLATiLE|VoMiT|VOSTFR|VoX|VoXHD|vrs|VS|VST|VT|W-B|w.0.0.f|w0rm|W23|W4F|WaCkOs|WAF|Wakku|WaLMaRT|WANKAZ|WASABi|WASHiPATO|WASTE|WastedTime|Wasurenai|Wasurenai_imuR88|WAT|Watakushi-Nameless|WATBATH|WATCHABLE|WATERS|WAVEY|WBZ|WEBSTER|Weby|WELEEF|WESEN|WEST|WESTSiDE|WHATELSE|WHEELS|WHiiZz|WhoKnow|WhyNot|WiDE|WiKi|WILDER|WinD|WinD-DE|WiNG|WingmanNZ|Wings13|WINTERS|WiRA|WiRE|WLF|WLM|WLX|WMZ|WNFN|WodkaE|woh|WoLF|WOLFPACK|Wolky|WOMBAT|WooDY|WoRKZ|WORLDVD|WPi|WRCR|WRD|WuSiWuG|WZF|X-Death|X-S|x0r|x264Crew|x4subs|XanaX|xander|XC|xD2V|xDR|Xell|XF|XiA|XII|XM4F|XMC|XOR|XORBiTANT|XOXO|XPERT|XPERT.HD|XPRESS|XR5|xRed|xRG|xRipp|XS|xSCR|XSHD|XSTREEM|XSubs|XTM|xTriLL|XTSF|xV|XviK|XWT|XXX4U|XYZ|Y-T|YA|YABAI|YaGAF|YAKU|YaKuZa|YanY|YAPG|YARDVID|YCDV|YellowBeast|YELLOWBiRD|YesMAX|YesTV|YFN|Ygt|YHBT|Yibis|YIFY|YingYang-Subs|YMG|Yn1D|YnK|YoHo|YOUFORGOTTOREPACKTHIS|Your-Mom|YRAG|YS|YSTeam|YT|YTS|YTS.AG|YUMA|YuS-m.3.3.w|YuS-MaiWaiFu|YuS-マイワイフ|Yuurisan-Subs|YY-S|Zalis|zan|ZBS|zdzd|ZeDD|ZEKTORM|ZEN|zenki|ZER0|ZeRO|Zero-Raws|ZEST|Zeus-Dias|ZKuS|zman|ZMG|zomg|Zomg-Killerhurtalot|ZON3|Zorori-Project|Zox|ZQ|ZSiSO|Zurako|Zuzuu|ZX|ZZGtv|~AA~|~Invincible|ßsZ(?i))");
                var formatRegex =
                    new Regex(
                        @"((?i)CAMRip|CAM|TS|TELESYNC|PDVD|PTVD|PPVRip|SCR|SCREENER|DVDSCR|DVDSCREENER|BDSCR|R4|R5|R5LINE|R5.LINE|DVD|DVD5|DVD9|DVDRip|DVDR|TVRip|DSR|PDTV|SDTV|HDTV|HDTVRip|DVB|DVBRip|DTHRip|VODRip|VODR|BDRip|BRRip|BR.Rip|BluRay|Blu.Ray|BD|BDR|BD25|BD50|3D.BluRay|3DBluRay|3DBD|Remux|BDRemux|BR.Scr|BR.Screener|HDDVD|HDRip|WorkPrint|VHS|VCD|TELECINE|WEBRip|WEB.Rip|WEBDL|WEB.DL|WEBCap|WEB.Cap|ithd|iTunesHD|Laserdisc|AmazonHD|NetflixHD|NetflixUHD|VHSRip|LaserRip|URip|UnknownRip|MicroHD|WP|TC|PPV|DDC|R5.AC3.5.1.HQ|DVD-Full|DVDFull|Full-Rip|FullRip|DSRip|SATRip|BD5|BD9|Extended|Uncensored|Remastered|Unrated|Uncut|IMAX|(Ultimate.)?(Director.?s|Theatrical|Ultimate|Final|Rogue|Collectors|Special|Despecialized).(Cut|Edition|Version)|((H|HALF|F|FULL)[^\\p{Alnum}]{0,2})?(SBS|TAB|OU)|DivX|Xvid|AVC|(x|h)[.]?(264|265)|HEVC|3ivx|PGS|MP[E]?G[45]?|MP[34]|(FLAC|AAC|AC3|DD|MA).?[2457][.]?[01]|[26]ch|(Multi.)?DTS(.HD)?(.MA)?|FLAC|AAC|AC3|TrueHD|Atmos|[M0]?(420|480|720|1080|1440|2160)[pi]|(?<=[-.])(420|480|720|1080|2D|3D)|10.?bit|(24|30|60)FPS|Hi10[P]?|[a-z]{2,3}.(2[.]0|5[.]1)|(19|20)[0-9]+(.)S[0-9]+(?!(.)?E[0-9]+)|(?<=\\d+)v[0-4]|CD\\d+|3D|2D|PROPER|LIMITED(?i))");
                var spRegex = new Regex(@"[sS][0-9]{2}[eE][0-9]{2}");
                var sXregex = new Regex(@"([0-9]{1,2}[xX][0-9]{2})");
                var threeRegex = new Regex(@"(\b\d{3}\b[.])");
                var fourRegex = new Regex(@"((19|20)[0-9][0-9])");
                foreach (var name in allfiles)
                {
                    var newName = "";
                    var original = Path.GetFileName(name);
                    var tmpFile = Path.GetFileNameWithoutExtension(name);
                    var ext = Path.GetExtension(name);
                    //var patern = new[]{"44"};
                    var index = ++i;
                    if (tmpFile != null)
                    {
                        var format = @"";
                        var group = @"";
                        tmpFile = tmpFile.EndsWith(".") ? tmpFile.Substring(0, tmpFile.Length - 1) : tmpFile;
                        if (spRegex.IsMatch(tmpFile)) newName = original;
                        else if (sXregex.IsMatch(tmpFile))
                        {
                            var sPattern = "[0-9]{1,2}[xX]";
                            var sRegex = Regex.Match(tmpFile, sPattern);
                            var se = sRegex.Value.ToLower();
                            var seSize = se.Length;
                            se = se.Replace("x", "");
                            se = se.Length > 0 && se.Length < 2 ? "S0" + se : "S" + se; 
                            sPattern = "[xX][0-9]{1,2}";
                            var epRegex = Regex.Match(tmpFile, sPattern);
                            var ep = epRegex.Value.ToLower();
                            var epSize = ep.Length;
                            ep = ep.Replace("x", "");
                            ep = ep.Length > 0 && ep.Length < 2 ? "E0" + ep : "E" + ep;
                            var sEep = se + ep;

                            sXregex = new Regex(@"([0-9]{1,2}[xX][0-9]{1,2}).*");
                            var filename = sXregex.Replace(tmpFile, string.Empty);
                            var last = filename[filename.Length - 1];
                            if (last.Equals('.')) filename = filename.Substring(0, filename.Length - 1);
                            tmpFile = tmpFile.Substring(filename.Length + seSize + epSize);
                            tmpFile = tmpFile.StartsWith(".") ? tmpFile.Substring(1) : tmpFile;
                            var tmp = tmpFile.Split('.', '-');
                            format = tmp.Where(item => TestWord(item, formatRegex)).Aggregate(format, (current, item) => current + item + ".");
                            format = format.Substring(0, format.Length - 1);
                            tmpFile = tmpFile.Substring(format.Length);
                            tmpFile = tmpFile.StartsWith("-") ? tmpFile.Substring(1) : tmpFile;
                            var tepShowName = string.Concat(tmpFile.TakeWhile(c => c != '['));
                            var showName = new string(tepShowName.ToArray());
                            group += TestWord(showName, groupRegex) ? showName : "";
                            if (!group.Equals(""))
                                group = !group.StartsWith("-") ? "-" + group : group;
                            newName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(filename) + "." + sEep + "." + format.ToUpper()  + group.ToUpper() + ext;
                            
                        }
                        else if (threeRegex.IsMatch(tmpFile))
                        {
                            const string yPattern = @"^(19|20)[0-9][0-9]";
                            const string s3Pattern = @"\b\d{3}\b";
                            const string s4Pattern = @"\b\d{4}\b";
                            var tmpRegex = new Regex(@"([0-9]{1,2}[0-9]{1,2}).*");
                            var filename = tmpRegex.Replace(tmpFile, string.Empty);
                            var last = filename[filename.Length - 1];
                            if (last.Equals('.')) filename = filename.Substring(0, filename.Length - 1);
                            tmpFile = tmpFile.Substring(filename.Length + 1);
                            var yRegex = Regex.Match(tmpFile, yPattern);
                            var year = yRegex.Value.ToLower();
                            if (year.Length > 0)
                                year += ".";
                                tmpFile = tmpFile.Substring(year.Length);
                            var sRegex = Regex.Match(tmpFile, s3Pattern);
                            if (!sRegex.Success) sRegex = Regex.Match(tmpFile, s4Pattern);
                            var sEep = sRegex.Value.ToLower();
                            var sEepSize = sEep.Length;
                            if (sEepSize != 0) 
                                sEep = sEep.Length > 3 ? "S" + sEep.Substring(0, 2) + "E" + sEep.Substring(2) +"." : "S0" + sEep.Substring(0, 1) + "E" + sEep.Substring(1)+ ".";
                            tmpFile = tmpFile.Substring(sEepSize);
                            tmpFile = tmpFile.StartsWith(".") ? tmpFile.Substring(1) : tmpFile;
                            var tmp = tmpFile.Split('.', '-');
                            format = tmp.Where(item => TestWord(item, formatRegex)).Aggregate(format, (current, item) => current + item + ".");
                            format = format.Substring(0, format.Length - 1);
                            tmpFile = tmpFile.Substring(format.Length);
                            tmpFile = tmpFile.StartsWith("-") ? tmpFile.Substring(1) : tmpFile;
                            var tepShowName = string.Concat(tmpFile.TakeWhile(c => c != '['));
                            var showName = new string(tepShowName.ToArray());
                            group += TestWord(showName, groupRegex) ? showName : "";
                            if (!group.Equals(""))
                                group = !group.StartsWith("-") ? "-"+ group : group;
                            newName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(filename) + "." + year + sEep + format.ToUpper() + group.ToUpper() + ext;
                            //newName = patern.Aggregate(newName, (current, item) => formatRegex.Replace(current, item));
                        }

                        else if (fourRegex.IsMatch(tmpFile))
                        {
                            const string yPattern = @"^(19|20)[0-9][0-9]";
                            const string s3Pattern = @"\b\d{3}\b[.]";
                            const string s4Pattern = @"\b\d{4}\b";
                            var tmpRegex = new Regex("(19|20)[0-9][0-9].*");
                            var filename = tmpRegex.Replace(tmpFile, string.Empty);
                            var last = filename[filename.Length - 1];
                            if (last.Equals('.')) filename = filename.Substring(0, filename.Length - 1);
                            tmpFile = tmpFile.Substring(filename.Length + 1);
                            var yRegex = Regex.Match(tmpFile, yPattern);
                            var year = yRegex.Value.ToLower();
                            if (year.Length > 0)
                                year += ".";
                                tmpFile = tmpFile.Substring(year.Length);
                            var sRegex = Regex.Match(tmpFile, s3Pattern);
                            if (!sRegex.Success) sRegex = Regex.Match(tmpFile, s4Pattern);
                            var sEep = sRegex.Value.ToLower();
                            var sEepSize = sEep.Length;
                            if (sEepSize != 0)
                                sEep = sEepSize > 3 ? "S" + sEep.Substring(0, 2) + "E" + sEep.Substring(2) + "." : "S0" + sEep.Substring(0, 1) + "E" + sEep.Substring(1) + ".";
                            tmpFile = tmpFile.Substring(sEepSize);
                            tmpFile = tmpFile.StartsWith(".") ? tmpFile.Substring(1) : tmpFile;
                            //var tmpstr = @"";
                            var tmp = tmpFile.Split('-');
                            format = tmp.Where(item => TestWord(item, formatRegex)).Aggregate(format, (current, item) => current + item + ".");
                            tmpFile = tmpFile.Substring(format.Length);
                            tmpFile = tmpFile.StartsWith("-") ? tmpFile.Substring(1) : tmpFile;
                            var tepShowName = string.Concat(tmpFile.TakeWhile(c => c != '['));
                            var showName = new string(tepShowName.ToArray());
                            group += TestWord(showName, groupRegex) ? showName : "";
                            if (!group.Equals(""))
                                group = !group.StartsWith("-") ? "-" + group : group;
                            newName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(filename) + "." + year + sEep + format.ToUpper() + group.ToUpper() + ext;
                            //newName = patern.Aggregate(newName, (current, item) => formatRegex.Replace(current, item));
                        }
                        else
                        {
                            var tmpFileSplit = tmpFile.Split('.');
                            var filename = "";
                            foreach (var item in tmpFileSplit)
                            {
                                if (!TestWord(item, formatRegex)) filename += item + ".";
                                else break;  
                            }
                            filename = filename.Substring(0, filename.Length - 1);
                            tmpFile = tmpFile.Substring(filename.Length + 1);
                            var tmp = tmpFile.Split('.', '-');
                            format = tmp.Where(item => TestWord(item, formatRegex)).Aggregate(format, (current, item) => current + item + ".");
                            format = format.Substring(0, format.Length - 1);
                            tmpFile = tmpFile.Substring(format.Length);
                            tmpFile = tmpFile.StartsWith("-") ? tmpFile.Substring(1) : tmpFile;
                            var tepShowName = string.Concat(tmpFile.TakeWhile(c => c != '['));
                            var showName = new string(tepShowName.ToArray());
                            group += TestWord(showName, groupRegex) ? showName : "";
                            if (!group.Equals(""))
                                group = !group.StartsWith("-") ? "-" + group : group;
                            newName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(filename) + "." + format.ToUpper() + group.ToUpper() + ext;
                            //newName = patern.Aggregate(newName, (current, item) => formatRegex.Replace(current, item));

                        }
                    }


                    listView.Items.Add(new ListViewItem(new[] { "", index.ToString(), original, newName}));
                    listView.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.HeaderSize);
                }
            }
        }

        private bool TestWord(string file, Regex filter)
        {
            return filter.IsMatch(file);
        }

        private static IList<string> GetFiles(string path, string pattern)
        {
            var files = new List<string>();

            try
            {
                files.AddRange(Directory.GetFiles(path, pattern, SearchOption.TopDirectoryOnly));
                foreach (var directory in Directory.GetDirectories(path)) files.AddRange(GetFiles(directory, pattern));
            }
            catch
            {
                Console.WriteLine(@"Opps!");
            }

            return files;
        }
    }
}
