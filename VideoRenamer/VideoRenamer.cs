using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using Newtonsoft.Json;
using TMDbLib.Client;
using RestSharp;
using TMDbLib.Objects.TvShows;


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

        private static IEnumerable<string> Filtered_List(IList<string> list)
        {
            return (from name in list let extension = Path.GetExtension(name) where extension != null let ext = extension.ToLower() where ext.Equals(".mp4") || ext.Equals(".avi") || ext.Equals(".mkv") || ext.Equals(".srt") select name).ToList();
        }

        private static Dictionary<string, string> _matches = new Dictionary<string, string>();
        private static Dictionary<string, string> _mark = new Dictionary<string, string>();

        //private bool _load = true;
        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //_load = true;
            listView.Items.Clear();
            _matches.Clear();
            var fs = new FolderSelectDialog();
            var result = fs.ShowDialog();
            if (!result) return;
            if (!Directory.Exists(fs.FileName)) return;
            var allfiles = Filtered_List(GetFiles(fs.FileName, "*.*"));
            foreach (var name in allfiles)
                ProcessName(name);
            //All_checkBox.Checked = true;
            //_load = false;

        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //_load = true;

            var fs = new OpenFileDialog
            {
                Title = @"Pick Some Files To ProcessName",
                Filter =
                    @"Supported File Types (*.mp4, *.mkv, *.avi, *.srt)|*.mp4; *.mkv; *.avi; *.srt",
                FilterIndex = 1,
                RestoreDirectory = false,
                Multiselect = true

            };
            var result = fs.ShowDialog();
            if (result != DialogResult.OK) return;
            var fileName = fs.FileNames;
            foreach (var name in fileName)
                ProcessName(name);
            //_load = false;
        }

        /*
                private static bool TestWord(string file, Regex filter)
                {
                    return filter.IsMatch(file);
                }
        */

        private static string SearchMatch(string input, string pattern)
        {
            var tmp = @"";
            var index = new List<int>();
            foreach (Match match in Regex.Matches(input, pattern))
            {
                tmp = match.Value;
                index.Add(input.IndexOf(tmp, StringComparison.OrdinalIgnoreCase));
            }
            return index.Count > 1 ? input.Substring(index[0], index[index.Count - 1] + tmp.Length - index[0]) : tmp;
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

        private readonly List<string> _list = new List<string>();
        private void ReName(string original, string newName)
        {
            try
            {
                File.Move(original, newName);

            }
            catch (Exception)
            {
                _list.Add(original);

            }

            //return _list;
        }

        private string ProcessNewName(string name)
        {
            var apiKeys = new ApiKeys();
            var original = Path.GetFileName(name);
            var tmpFile = Path.GetFileNameWithoutExtension(name);
            var ext = Path.GetExtension(name);
            var newName = "";
            var tmdBapikey = apiKeys.TmdBapikey;
            var tmdb = new TMDbClient(tmdBapikey);

            if (tmpFile != null)
            {
                tmpFile = tmpFile.EndsWith(".") ? tmpFile.Substring(0, tmpFile.Length - 1) : tmpFile;
                var index = 0;
                var title = extract_Title(tmpFile);
                index += title.Length;
                var year = GetYear(tmpFile.Substring(index));
                index += year.Length;
                var sEepObj = extract_seEp(tmpFile.Substring(index));
                var sEep = (string)sEepObj[0];
                index += (int)sEepObj[1];
                var format = extract_Format(tmpFile.Substring(index));
                index += format.Length;
                var group = extract_Group(tmpFile.Substring(index));
                var startWith = extract_startWite(tmpFile.Substring(index));
                var epName = GetEpName(title.TrimEnd('.').Replace(".", " "), tmdb, sEep, year);
                if (original != null && !string.IsNullOrEmpty(epName) && original.ToLower().Contains(epName.ToLower()))
                    newName = original;
                else
                    newName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title) + year + sEep + epName + format + startWith + group + ext;



            }

            var match = Regex.Match(newName, "(X|H)?(264|265)");
            var ma = match.Value;
            newName = Regex.Replace(newName, ma, ma.ToLower());
            if (newName.Contains("HDTV") && !newName.Contains("XviD") && !Regex.IsMatch(newName, "([xX]|[hH])?(264|265)"))
                newName = newName.Replace("HDTV", "HDTV.x264");
            if (original != null) _matches.Add(original, newName);

            return newName;
        }

        private dynamic GetJson(string uri)
        {
            var client = new RestClient(uri);
            var request = new RestRequest { Method = Method.GET };
            request.AddHeader("Accept", "application/json");
            request.Parameters.Clear();
            var response = client.Execute(request);
            //var deserializeObjectJson = JsonConvert.DeserializeObject<dynamic>(response.Content);
            return JsonConvert.DeserializeObject<dynamic>(response.Content);
            //return deserializeObjectJson.results;
        }
        private string GetEpName(string title, TMDbClient tmdb, string seEp = "", string year = "0")
        {
            if (_mark.ContainsValue(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title)))
            {
                var tmpYear = (_mark.FirstOrDefault(z => z.Value.ToLower().Equals(title)).Key).Split('-');
                year = tmpYear[0];
            }
               
            var potential = new List<dynamic>();
            var sEepNum = seEp.ToUpper().TrimEnd('.').TrimStart('S').Split('E');
            if (string.IsNullOrEmpty(seEp)) return "";
            var tvUri = $@"https://api.themoviedb.org/3/search/tv?api_key={tmdb.ApiKey}&language=en-US&query={title}&page=1&first_air_date_year={year}";
            var response = GetJson(tvUri);
            foreach (var result in response.results)
            {
                if (((string)result.name).ToLower().Equals(title.ToLower()))
                {
                    potential.Add(result);
                }
            }
            if (potential.Count == 1 || AndroidStyleToggleSwitch.Checked)
            {
                var uri = $@"https://api.themoviedb.org/3/tv/{potential[0].id}/season/{sEepNum[0]}/episode/{sEepNum[1]}?api_key={tmdb.ApiKey}";
                var tvJson = GetJson(uri);
                var delimitersStrings = new[] { "-", "_", "+", "." };
                var isDelimiter = delimitersStrings.Any(delimit => ((string)tvJson.name).Contains(delimit));
                return isDelimiter ? ((string)tvJson.name).Replace(" ", string.Empty) + "." : ((string)tvJson.name).Replace(" ", ".") + ".";

            }

            if (potential.Count == 0) return "";

            var msg = new MsgBox();
            const string imdburl = "http://www.imdb.com/title/";
            const int x = 25;
            var y = 30;
            foreach (var potentialItem in potential)
            {
                var id = (int)potentialItem.id;
                var imdbId = $@"{tmdb.GetTvShowAsync(id, TvShowMethods.ExternalIds).Result.ExternalIds.ImdbId}";
                msg.AddLabeles(imdburl + imdbId, $@"{(string)potentialItem.original_name} ({(string)potentialItem.first_air_date})", x, y);
                y += 20;
            }
            msg.MyTitle = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title);
            msg.ShowDialog();
            foreach (var selected in potential)
            {
                var imdbId = tmdb.GetTvShowAsync(((int)selected.id), TvShowMethods.ExternalIds).Result.ExternalIds.ImdbId;
                if (!imdbId.Equals(msg.MyId)) continue;
                var uri = $@"https://api.themoviedb.org/3/tv/{potential[0].id}/season/{sEepNum[0]}/episode/{sEepNum[1]}?api_key={tmdb.ApiKey}";
                var tvJson = GetJson(uri);
                //return tvJson.name;
                var delimitersStrings = new[] { "-", "_", "+", "." };
                var isDelimiter = delimitersStrings.Any(delimit => ((string)tvJson.name).Contains(delimit));
                _mark.Add((string)selected.first_air_date, (string)selected.original_name);
                return isDelimiter ? ((string)tvJson.name).Replace(" ", string.Empty) + "." : ((string)tvJson.name).Replace(" ", ".") + ".";
            }

            return "";

        }

        private static string extract_Group(string tmpFile)
        {
            var group = SearchMatch(tmpFile, Matches.GroupRegex.ToString());
            var gDict = Matches.GroupList.ToDictionary(x => x, StringComparer.CurrentCultureIgnoreCase);
            group = Regex.Replace(group, @"[^\>]*", x =>
            {
                var tmp = x.Value.StartsWith("-") ? x.Value.Substring(1) : x.Value;
                string replace;
                gDict.TryGetValue(tmp, out replace);
                return replace ?? tmp;
            });
            return group;
        }


        private static string extract_Format(string tmpFile)
        {
            var format = SearchMatch(tmpFile, Matches.FormatRegex.ToString());
            var fDict = Matches.FormatList.ToDictionary(x => x, StringComparer.CurrentCultureIgnoreCase);
            format = Regex.Replace(format.ToUpper(), @"\b.+?\b", x =>
            {
                fDict.TryGetValue(x.Value, out string replace);
                return replace ?? x.Value;
            });
            var fMatch = Regex.Match(format, "(X|H)?(264|265)");
            var mc = fMatch.Value;
            return Regex.Replace(format, mc, mc.ToLower());
        }

        private static string extract_startWite(string tmpFile)
        {
            if (tmpFile.StartsWith("-"))
            {
                return @"-";

            }

            return tmpFile.StartsWith(".") ? @"." : "";
        }


        private static object[] extract_seEp(string tmpFile)
        {
            foreach (var match in Matches.seEp_dic)
            {
                var epRegex = Regex.Match(tmpFile, match.Value.ToString().Replace(".*", string.Empty));
                if (epRegex.Success)
                {
                    switch (match.Key)
                    {

                        case "se":

                            {
                                var sEep = epRegex.Value;
                                var sEepSize = sEep.Length;
                                if (sEepSize != 0)
                                    return new object[] { sEep.ToUpper() + ".", 7 };


                            }
                            break;

                        case "NxM":
                            {
                                var sPattern = "[0-9]{1,2}[xX]";
                                var sRegex = Regex.Match(tmpFile, sPattern);
                                var se = sRegex.Value.ToLower();
                                se = se.Replace("x", "");
                                se = se.Length > 0 && se.Length < 2 ? "S0" + se : "S" + se;
                                sPattern = "[xX][0-9]{1,2}";
                                var _epRegex = Regex.Match(tmpFile, sPattern);
                                var ep = _epRegex.Value.ToLower();
                                var epSize = ep.Length;
                                ep = ep.Replace("x", "");
                                ep = epSize > 0 && ep.Length < 2 ? "E0" + ep : "E" + ep;
                                if (se.Length > 0 && ep.Length > 0)
                                    //return se + ep + ".";
                                    return new object[] {  se + ep + ".", sRegex.Value.Length + _epRegex.Value.Length};
                            }
                            break;

                        case "3d":
                        case "4d":
                            {
                                const string s3Pattern = @"\b\d{3}\b";
                                const string s4Pattern = @"\b\d{4}\b";
                                //var tmpRegex = new Regex(@"([0-9]{1,2}[0-9]{1,2}).*");
                                //var filename = tmpRegex.Replace(tmpFile, string.Empty);
                                //if (filename.EndsWith(".")) filename = filename.Substring(0, filename.Length - 1);
                                //tmpFile = tmpFile.Substring(filename.Length + 1);
                                var sRegex = Regex.Match(tmpFile, s3Pattern);
                                if (!sRegex.Success) sRegex = Regex.Match(tmpFile, s4Pattern);
                                var sEep = sRegex.Value.ToLower();
                                var sEepSize = sEep.Length;
                                if (sEepSize != 0)
                                    return sEepSize > 3
                                        ? new object[] { "S" + sEep.Substring(0, 2) + "E" + sEep.Substring(2) + ".", 5 }
                                        : new object [] { "S0" + sEep.Substring(0, 1) + "E" + sEep.Substring(1) + ".", 4 };

                            }
                            break;

                        case "other":
                            {
                                const string m4Pattern = @"\b\d{5,10}\b";
                                var tmpRegex = new Regex(@"\b\d{5,10}\b.*");
                                var filename = tmpRegex.Replace(tmpFile, string.Empty);
                                if (filename.EndsWith(".")) filename = filename.Substring(0, filename.Length - 1);
                                tmpFile = tmpFile.Substring(filename.Length + 1).ToLower();
                                var sRegex = Regex.Match(tmpFile, m4Pattern);
                                var sEep = sRegex.Value.ToLower();
                                var sEepSize = sEep.Length;
                                if (sEepSize != 0)
                                    return new object[] { "S0" + sEep.Substring(0, 1) + "E" + sEep.Substring(1) + ".", 3 };
                            }
                            break;
                        default:
                            return new object[]{"",0};

                    }
                }
            }

            return new object[] {"",0};

        }

        private static string GetYear(string tmpFile)
        {
            var match = Regex.Match(tmpFile, Matches.YRegex);

            return match.Success ? match.Value + "." : "";
        }

        private static string extract_Title(string tmpFile)
        {

            //var title = year.Length > 0 ? tmpFile.Replace(year, string.Empty) : tmpFile;
            foreach (var match in Matches.seEp_dic)
            {
                if (!match.Value.IsMatch(tmpFile)) continue;
                //title = match.Value.Replace(title, string.Empty);
                //return title.Contains("..") ? title.Substring(0,title.Length - 1) : title;
                return match.Value.Replace(tmpFile, string.Empty);

            }
            return Regex.Replace(tmpFile, $"{Matches.YRegex}.*", string.Empty);
        }

        private void ProcessName(string name)
        {

            var i = listView.Items.Count;
            var original = Path.GetFileName(name);
            var directoryName = Path.GetDirectoryName(name);
            var index = ++i;
            if (name == null) return;
            if (listView.FindItemWithText(Path.GetFileName(name)) == null)
            {
                var newName = ProcessNewName(name);
                listView.ItemChecked -= listView_ItemChecked;
                listView.Items.Add(new ListViewItem(new[] { "", index.ToString(), original, newName, directoryName }));
                listView.ItemChecked += listView_ItemChecked;
                if (!listView.Items[index - 1].SubItems[2].Text.Equals(listView.Items[index - 1].SubItems[3].Text))
                {
                    listView.Items[index - 1].Checked = true;
                    listView.Items[index - 1].UseItemStyleForSubItems = false;
                    listView.Items[index - 1].SubItems[3].ForeColor = Color.Red;
                }
                else
                {
                    listView.Items[index - 1].SubItems[3].Text = "";
                }
            }

            listView.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void VideoRenamer_Shown(object sender, EventArgs e)
        {
            CheckForUpdates.RunWorkerAsync(true);
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckForUpdates.RunWorkerAsync(false);
        }

        private void All_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            listView.ItemChecked -= listView_ItemChecked;
            if (All_checkBox.Checked)
            {
                for (var i = 0; i < listView.Items.Count; i++)
                {
                    listView.Items[i].Checked = true;
                    //listView.Items[i].SubItems[3].Text = ProcessNewName(listView.Items[i].SubItems[2].Text);
                    listView.Items[i].SubItems[3].Text = _matches[listView.Items[i].SubItems[2].Text];
                    if (!listView.Items[i].SubItems[2].Text.Equals(listView.Items[i].SubItems[3].Text))
                    {
                        listView.Items[i].UseItemStyleForSubItems = false;
                        listView.Items[i].SubItems[3].ForeColor = Color.Red;
                    }
                    else
                    {
                        listView.Items[i].UseItemStyleForSubItems = false;
                        listView.Items[i].SubItems[3].ForeColor = Color.Black;
                    }
                }
            }
            else
            {
                for (var i = 0; i < listView.Items.Count; i++)
                {
                    listView.Items[i].Checked = false;
                    listView.Items[i].SubItems[3].Text = "";
                }
            }

            listView.ItemChecked += listView_ItemChecked;
        }

        private void Rename_button_Click(object sender, EventArgs e)
        {
            //_load = true;
            _list.Clear();
            var errors = @"";
            foreach (ListViewItem item in listView.Items)
            {
                if (item.Checked)
                {
                    var original = item.SubItems[4].Text + "\\" + item.SubItems[2].Text;
                    var newName = item.SubItems[4].Text + "\\" + item.SubItems[3].Text;
                    ReName(original, newName);
                    item.SubItems[2].Text = item.SubItems[3].Text;
                    item.SubItems[3].Text = "";
                    item.Checked = false;
                }
            }
            All_checkBox.Checked = false;
            if (_list.Count > 0)
            {
                errors = _list.Aggregate(errors, (current, err) => current + err + Environment.NewLine);
                MessageBox.Show(@"Done But with Errors." + Environment.NewLine + @"The Following Couldn't Be Renamed:" + Environment.NewLine + errors, @"Done");
            }

            else MessageBox.Show(@"Done.", @"Done");
        }


        private void listView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            //if (listView.FocusedItem != null)
            //{
            foreach (ListViewItem item in listView.Items)
            {
                if (item.Checked)
                {
                    //item.SubItems[3].Text = ProcessNewName(item.SubItems[2].Text);
                    item.SubItems[3].Text = _matches[item.SubItems[2].Text];
                    if (!item.SubItems[2].Text.Equals(item.SubItems[3].Text))
                    {
                        item.UseItemStyleForSubItems = false;
                        item.SubItems[3].ForeColor = Color.Red;
                    }
                    else
                    {
                        item.UseItemStyleForSubItems = false;
                        item.SubItems[3].ForeColor = Color.Black;
                    }
                }

                else
                {
                    item.SubItems[3].Text = "";
                }
            }
            //}
        }


        private void CheckForUpdates_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var downloadUrl = @"";
            Version newVersion = null;
            var xmlUrl = @"https://oribenhur.github.io/update.xml";
            Version appVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            var appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            var doc = XDocument.Load(xmlUrl);
            try
            {
                foreach (var dm in doc.Descendants(appName))
                {
                    var versionElement = dm.Element(@"version");
                    if (versionElement == null) continue;
                    var urlelEment = dm.Element(@"url");
                    if (urlelEment == null) continue;
                    newVersion = new Version(versionElement.Value);
                    downloadUrl = urlelEment.Value;

                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

            if (appVersion.CompareTo(newVersion) < 0)
            {
                var result = MessageBox.Show(
                    $@"{appName} v.{newVersion} is out!{Environment.NewLine}Would You Like To Download It?", @"New Version is available", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    System.Diagnostics.Process.Start(downloadUrl);
            }
            else
            {
                if (!(bool)e.Argument)
                    MessageBox.Show(@"You Are Running The Last Version.", @"No New Updates");
            }
        }
    }
}



//if (Matches.SpRegex.IsMatch(tmpFile))
//{
//    var filename = "";
//    const string sEepPattern = @"[Ss][0-9]{2}[Ee][0-9]{2}";
//    //const string yPattern = @"^(19|20)[0-9][0-9]";
//    var tmpRegex = new Regex(@"([0-9]{1,2}[0-9]{1,2}).*");
//    var sEepMatch = Regex.Match(tmpFile, sEepPattern);
//    var sEep = sEepMatch.Value;
//    var sEepSize = sEep.Length;
//    if (sEepSize != 0)
//    {
//        tmpFile = tmpFile.Replace(sEep + ".", string.Empty);
//        sEep = sEep.ToUpper() + ".";
//    }
//    var tmpFileSplit = tmpFile.Split('.');
//    foreach (var item in tmpFileSplit)
//    {
//        if (!TestWord(item, Matches.FormatRegex)) filename += item + ".";
//        else break;
//    }
//    filename = filename.Substring(0, filename.Length);
//    var yRegex = Regex.Match(filename, tmpRegex.ToString());
//    var year = yRegex.Value.ToLower();
//    filename = tmpRegex.Replace(filename, string.Empty);
//    tmpFile = year.Length> 0 ?  tmpFile.Substring(filename.Length + year.Length - 1) : tmpFile.Substring(filename.Length + year.Length);
//    format = SearchMatch(tmpFile, Matches.FormatRegex.ToString());
//    format = format.Substring(0, format.Length - 1);
//    tmpFile = tmpFile.StartsWith("-") ? tmpFile.Substring(1) : tmpFile;
//    var fDict = Matches.FormatList.ToDictionary(x => x, StringComparer.CurrentCultureIgnoreCase);
//    format = Regex.Replace(format.ToUpper(), @"\b.+?\b", x =>
//    {
//        string replace;
//        fDict.TryGetValue(x.Value, out replace);
//        return replace ?? x.Value;
//    });
//    var fMatch = Regex.Match(format, "(X|H)?(264|265)");
//    var mc = fMatch.Value;
//    format = Regex.Replace(format, mc, mc.ToLower());
//    tmpFile = tmpFile.Substring(format.Length);
//    var startwith = @"";
//    if (tmpFile.StartsWith("-"))
//    {
//        startwith = @"-";
//        tmpFile = tmpFile.Substring(1);
//    }
//    else if (tmpFile.StartsWith("."))
//    {
//        startwith = @".";
//        tmpFile = tmpFile.Substring(1);
//    }


//    group = SearchMatch(tmpFile, Matches.GroupRegex.ToString());
//    var gDict = Matches.GroupList.ToDictionary(x => x, StringComparer.CurrentCultureIgnoreCase);
//    group = Regex.Replace(group, @"[^\>]*", x =>
//    {
//        var tmp = x.Value.StartsWith("-") ? x.Value.Substring(1) : x.Value;
//        string replace;
//        gDict.TryGetValue(tmp, out replace);
//        return replace ?? tmp;
//    });
//    var epName = "";
//    var sEepNum = sEep.ToUpper().TrimStart('S').Split('E');
//    var cleanFilename = filename.TrimEnd('.').Replace('.', ' ');
//    var searchResults = tmdb.SearchTvShowAsync(cleanFilename).Result;
//    foreach (var item in searchResults.Results)
//    {
//        if (!item.Name.ToLower().Equals(cleanFilename.ToLower())) continue;
//        var uri =
//            $"https://api.themoviedb.org/3/tv/{item.Id}/season/{sEepNum[0]}/episode/{sEepNum[1]}?api_key={tmdBapikey}";
//        var client = new RestClient(uri);
//        var request = new RestRequest { Method = Method.GET };
//        request.AddHeader("Accept", "application/json");
//        request.Parameters.Clear();
//        var response = client.Execute(request);
//        var deserializeObjectJson = JsonConvert.DeserializeObject(response.Content);
//        var token = JObject.Parse(deserializeObjectJson.ToString());
//        var result = token.SelectToken("name").Value<string>();
//        var delimitersStrings = new [] {"-", "_","+", "."};
//        var isDelimiter = delimitersStrings.Any(delimit => result.Contains(delimit));

//        epName = isDelimiter ? result.Replace(" ", string.Empty) + "." : result.Replace(" ", ".") + ".";
//        break;
//    }
//    newName = original != null && original.ToLower().Contains(epName.ToLower()) ? original : CultureInfo.CurrentCulture.TextInfo.ToTitleCase(filename) + year + sEep + epName + format + startwith + group + ext;
//}
//else if (Matches.SXregex.IsMatch(tmpFile))
//{
//    var sPattern = "[0-9]{1,2}[xX]";
//    var tmpRegex = new Regex(@"([0-9]{1,2}[0-9]{1,2}).*");
//    var sRegex = Regex.Match(tmpFile, sPattern);
//    var se = sRegex.Value.ToLower();
//    var seSize = se.Length;
//    se = se.Replace("x", "");
//    se = se.Length > 0 && se.Length < 2 ? "S0" + se : "S" + se;
//    sPattern = "[xX][0-9]{1,2}";
//    var epRegex = Regex.Match(tmpFile, sPattern);
//    var ep = epRegex.Value.ToLower();
//    var epSize = ep.Length;
//    ep = ep.Replace("x", "");
//    ep = ep.Length > 0 && ep.Length < 2 ? "E0" + ep : "E" + ep;
//    var sEep = se + ep;

//    Matches.SXregex = new Regex(@"([0-9]{1,2}[xX][0-9]{1,2}).*");
//    var filename = Matches.SXregex.Replace(tmpFile, string.Empty);
//    //if (filename.EndsWith(".")) filename = filename.Substring(0, filename.Length - 1);
//    var yRegex = Regex.Match(filename, tmpRegex.ToString());
//    var year = yRegex.Value.ToLower();
//    filename = tmpRegex.Replace(filename, string.Empty);
//    tmpFile = year.Length > 0 ? tmpFile.Substring(filename.Length + year.Length + seSize + epSize) : tmpFile.Substring(filename.Length + seSize + epSize);
//    tmpFile = tmpFile.StartsWith(".") ? tmpFile.Substring(1) : tmpFile;
//    format = SearchMatch(tmpFile, Matches.FormatRegex.ToString());
//    var fDict = Matches.FormatList.ToDictionary(x => x, StringComparer.CurrentCultureIgnoreCase);
//    format = Regex.Replace(format.ToUpper(), @"\b.+?\b", x =>
//    {
//        fDict.TryGetValue(x.Value, out string replace);
//        return replace ?? x.Value;
//    });
//    var fMatch = Regex.Match(format, "(X|H)?(264|265)");
//    var mc = fMatch.Value;
//    format = Regex.Replace(format, mc, mc.ToLower());
//    tmpFile = tmpFile.Substring(format.Length);
//    var startwith = @"";
//    if (tmpFile.StartsWith("-"))
//    {
//        startwith = @"-";
//        tmpFile = tmpFile.Substring(1);
//    }
//    else if (tmpFile.StartsWith("."))
//    {
//        startwith = @".";
//        tmpFile = tmpFile.Substring(1);
//    }
//    group = SearchMatch(tmpFile, Matches.GroupRegex.ToString());
//    var gDict = Matches.GroupList.ToDictionary(x => x, StringComparer.CurrentCultureIgnoreCase);
//    group = Regex.Replace(group, @"[^\>]*", x =>
//    {
//        var tmp = x.Value.StartsWith("-") ? x.Value.Substring(1) : x.Value;
//        string replace;
//        gDict.TryGetValue(tmp, out replace);
//        return replace ?? tmp;
//    });
//    var epName = "";
//    var cleanFilename = filename.TrimEnd('.').Replace('.', ' ');
//    var searchResults = tmdb.SearchTvShowAsync(cleanFilename).Result;
//    foreach (var item in searchResults.Results)
//    {
//        if (!item.Name.ToLower().Equals(cleanFilename.ToLower())) continue;
//        var uri =
//            $"https://api.themoviedb.org/3/tv/{item.Id}/season/{se.TrimStart('S')}/episode/{ep.TrimStart('E')}?api_key={tmdBapikey}";
//        var client = new RestClient(uri);
//        var request = new RestRequest { Method = Method.GET };
//        request.AddHeader("Accept", "application/json");
//        request.Parameters.Clear();
//        var response = client.Execute(request);
//        var deserializeObjectJson = JsonConvert.DeserializeObject(response.Content);
//        var token = JObject.Parse(deserializeObjectJson.ToString());
//        var result = token.SelectToken("name").Value<string>();
//        var delimitersStrings = new[] { "-", "_", "+", "." };
//        var isDelimiter = delimitersStrings.Any(delimit => result.Contains(delimit));

//        epName = isDelimiter ? result.Replace(" ", string.Empty) : result.Replace(" ", ".");
//        break;
//    }
//    newName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(filename) + year + sEep + "." + epName + '.' + format + startwith + group + ext;

//}
//else if (Matches.ThreeRegex.IsMatch(tmpFile))
//{
//    const string yPattern = @"^(19|20)[0-9][0-9]";
//    const string s3Pattern = @"\b\d{3}\b";
//    const string s4Pattern = @"\b\d{4}\b";
//    var tmpRegex = new Regex(@"([0-9]{1,2}[0-9]{1,2}).*");
//    var filename = tmpRegex.Replace(tmpFile, string.Empty);
//    if (filename.EndsWith(".")) filename = filename.Substring(0, filename.Length - 1);
//    tmpFile = tmpFile.Substring(filename.Length + 1);
//    var yRegex = Regex.Match(tmpFile, yPattern);
//    var year = yRegex.Value.ToLower();
//    if (year.Length > 0)
//        year += ".";
//    tmpFile = tmpFile.Substring(year.Length);
//    var sRegex = Regex.Match(tmpFile, s3Pattern);
//    if (!sRegex.Success) sRegex = Regex.Match(tmpFile, s4Pattern);
//    var sEep = sRegex.Value.ToLower();
//    var sEepSize = sEep.Length;
//    if (sEepSize != 0)
//        sEep = sEep.Length > 3 ? "S" + sEep.Substring(0, 2) + "E" + sEep.Substring(2) + "." : "S0" + sEep.Substring(0, 1) + "E" + sEep.Substring(1) + ".";
//    tmpFile = tmpFile.Substring(sEepSize);
//    tmpFile = tmpFile.StartsWith(".") ? tmpFile.Substring(1) : tmpFile;
//    format = SearchMatch(tmpFile, Matches.FormatRegex.ToString());
//    var fDict = Matches.FormatList.ToDictionary(x => x, StringComparer.CurrentCultureIgnoreCase);
//    format = Regex.Replace(format.ToUpper(), @"\b.+?\b", x =>
//    {
//        string replace;
//        fDict.TryGetValue(x.Value, out replace);
//        return replace ?? x.Value;
//    });
//    var fMatch = Regex.Match(format, "(X|H)?(264|265)");
//    var mc = fMatch.Value;
//    format = Regex.Replace(format, mc, mc.ToLower());
//    tmpFile = tmpFile.Substring(format.Length);
//    var startwith = @"";
//    if (tmpFile.StartsWith("-"))
//    {
//        startwith = @"-";
//        tmpFile = tmpFile.Substring(1);
//    }
//    else if (tmpFile.StartsWith("."))
//    {
//        startwith = @".";
//        tmpFile = tmpFile.Substring(1);
//    }
//    group = SearchMatch(tmpFile, Matches.GroupRegex.ToString());
//    var gDict = Matches.GroupList.ToDictionary(x => x, StringComparer.CurrentCultureIgnoreCase);
//    group = Regex.Replace(group, @"[^\>]*", x =>
//    {
//        var tmp = x.Value.StartsWith("-") ? x.Value.Substring(1) : x.Value;
//        string replace;
//        gDict.TryGetValue(tmp, out replace);
//        return replace ?? tmp;
//    });
//    var epName = "";
//    var sEepNum = sEep.TrimStart('S').Split('E');
//    var cleanFilename = filename.TrimEnd('.').Replace('.', ' ');
//    var searchResults = tmdb.SearchTvShowAsync(cleanFilename).Result;
//    foreach (var item in searchResults.Results)
//    {
//        if (!item.Name.ToLower().Equals(cleanFilename.ToLower())) continue;
//        var uri =
//            $"https://api.themoviedb.org/3/tv/{item.Id}/season/{sEepNum[0]}/episode/{sEepNum[1]}?api_key={tmdBapikey}";
//        var client = new RestClient(uri);
//        var request = new RestRequest { Method = Method.GET };
//        request.AddHeader("Accept", "application/json");
//        request.Parameters.Clear();
//        var response = client.Execute(request);
//        if (response.StatusCode != HttpStatusCode.OK) break;
//        var deserializeObjectJson = JsonConvert.DeserializeObject(response.Content);
//        var token = JObject.Parse(deserializeObjectJson.ToString());
//        var result = token.SelectToken("name").Value<string>();
//        var delimitersStrings = new[] { "-", "_", "+", "." };
//        var isDelimiter = delimitersStrings.Any(delimit => result.Contains(delimit));

//        epName = isDelimiter ? result.Replace(" ", string.Empty) +"." : result.Replace(" ", ".") + ".";
//        break;
//    }
//    newName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(filename) + "." + year + sEep + epName + format + startwith + group + ext;
//}
//else if (Matches.FourRegex.IsMatch(tmpFile))
//{
//    const string yPattern = @"^(19|20)[0-9][0-9]";
//    const string s3Pattern = @"\b\d{3}\b";
//    const string s4Pattern = @"\b\d{4}\b";
//    var tmpRegex = new Regex("(19|20)[0-9][0-9].*");
//    var filename = tmpRegex.Replace(tmpFile, string.Empty);
//    if (filename.EndsWith(".")) filename = filename.Substring(0, filename.Length - 1);
//    tmpFile = tmpFile.Substring(filename.Length + 1).ToLower();
//    var yRegex = Regex.Match(tmpFile, yPattern);
//    var year = yRegex.Value.ToLower();
//    if (year.Length > 0)
//        year += ".";
//    tmpFile = tmpFile.Substring(year.Length);
//    var sRegex = Regex.Match(tmpFile, s3Pattern);
//    if (!sRegex.Success) sRegex = Regex.Match(tmpFile, s4Pattern);
//    var sEep = sRegex.Value.ToLower();
//    var sEepSize = sEep.Length;
//    if (sEepSize != 0)
//        sEep = sEepSize > 3 ? "S" + sEep.Substring(0, 2) + "E" + sEep.Substring(2) + "." : "S0" + sEep.Substring(0, 1) + "E" + sEep.Substring(1) + ".";
//    tmpFile = tmpFile.Substring(sEepSize);
//    tmpFile = tmpFile.StartsWith(".") ? tmpFile.Substring(1) : tmpFile;
//    format = SearchMatch(tmpFile, Matches.FormatRegex.ToString());
//    var fDict = Matches.FormatList.ToDictionary(x => x, StringComparer.CurrentCultureIgnoreCase);
//    format = Regex.Replace(format.ToUpper(), @"\b.+?\b", x =>
//    {
//        string replace;
//        fDict.TryGetValue(x.Value, out replace);
//        return replace ?? x.Value;
//    });
//    var fMatch = Regex.Match(format, "(X|H)?(264|265)");
//    var mc = fMatch.Value;
//    format = Regex.Replace(format, mc, mc.ToLower());
//    tmpFile = tmpFile.Substring(format.Length);
//    var startwith = @"";
//    if (tmpFile.StartsWith("-"))
//    {
//        startwith = @"-";
//        tmpFile = tmpFile.Substring(1);
//    }
//    else if (tmpFile.StartsWith("."))
//    {
//        startwith = @".";
//        tmpFile = tmpFile.Substring(1);
//    }
//    group = SearchMatch(tmpFile, Matches.GroupRegex.ToString());
//    var gDict = Matches.GroupList.ToDictionary(x => x, StringComparer.CurrentCultureIgnoreCase);
//    group = Regex.Replace(group, @"[^\>]*", x =>
//    {
//        var tmp = x.Value.StartsWith("-") ? x.Value.Substring(1) : x.Value;
//        string replace;
//        gDict.TryGetValue(tmp, out replace);
//        return replace ?? tmp;
//    });
//    var epName = "";
//    var sEepNum = sEep.TrimStart('S').Split('E');
//    var cleanFilename = filename.TrimEnd('.').Replace('.', ' ');
//    var searchResults = tmdb.SearchTvShowAsync(cleanFilename).Result;
//    foreach (var item in searchResults.Results)
//    {
//        if (!item.Name.ToLower().Equals(cleanFilename.ToLower())) continue;
//        var uri =
//            $"https://api.themoviedb.org/3/tv/{item.Id}/season/{sEepNum[0]}/episode/{sEepNum[1]}?api_key={tmdBapikey}";
//        var client = new RestClient(uri);
//        var request = new RestRequest { Method = Method.GET };
//        request.AddHeader("Accept", "application/json");
//        request.Parameters.Clear();
//        var response = client.Execute(request);
//        var deserializeObjectJson = JsonConvert.DeserializeObject(response.Content);
//        var token = JObject.Parse(deserializeObjectJson.ToString());
//        var result = token.SelectToken("name").Value<string>();
//        var delimitersStrings = new[] { "-", "_", "+", "." };
//        var isDelimiter = delimitersStrings.Any(delimit => result.Contains(delimit));

//        epName = isDelimiter ? result.Replace(" ", string.Empty) : result.Replace(" ", ".");
//        break;
//    }
//    newName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(filename) + "." + year + sEep + epName + format + startwith + group + ext;

//}
//else if (Matches.MorethenfourRegex.IsMatch(tmpFile))
//{
//    const string m4Pattern = @"\b\d{5,10}\b";
//    var tmpRegex = new Regex(@"\b\d{5,10}\b.*");
//    var filename = tmpRegex.Replace(tmpFile, string.Empty);
//    if (filename.EndsWith(".")) filename = filename.Substring(0, filename.Length - 1);
//    tmpFile = tmpFile.Substring(filename.Length + 1).ToLower();

//    var sRegex = Regex.Match(tmpFile, m4Pattern);
//    var sEep = sRegex.Value.ToLower();
//    var sEepSize = sEep.Length;
//    if (sEepSize != 0)
//        sEep = "S0" + sEep.Substring(0, 1) + "E" + sEep.Substring(1) + ".";
//    tmpFile = tmpFile.Substring(sEepSize);
//    tmpFile = tmpFile.StartsWith(".") ? tmpFile.Substring(1) : tmpFile;
//    format = SearchMatch(tmpFile, Matches.FormatRegex.ToString());
//    var fDict = Matches.FormatList.ToDictionary(x => x, StringComparer.CurrentCultureIgnoreCase);
//    format = Regex.Replace(format.ToUpper(), @"\b.+?\b", x =>
//    {
//        string replace;
//        fDict.TryGetValue(x.Value, out replace);
//        return replace ?? x.Value;
//    });
//    var fMatch = Regex.Match(format, "(X|H)?(264|265)");
//    var mc = fMatch.Value;
//    format = Regex.Replace(format, mc, mc.ToLower());
//    tmpFile = tmpFile.Substring(format.Length);
//    var startwith = @"";
//    if (tmpFile.StartsWith("-"))
//    {
//        startwith = @"-";
//        tmpFile = tmpFile.Substring(1);
//    }
//    else if (tmpFile.StartsWith("."))
//    {
//        startwith = @".";
//        tmpFile = tmpFile.Substring(1);
//    }
//    group = SearchMatch(tmpFile, Matches.GroupRegex.ToString());
//    var gDict = Matches.GroupList.ToDictionary(x => x, StringComparer.CurrentCultureIgnoreCase);
//    group = Regex.Replace(group, @"[^\>]*", x =>
//    {
//        var tmp = x.Value.StartsWith("-") ? x.Value.Substring(1) : x.Value;
//        string replace;
//        gDict.TryGetValue(tmp, out replace);
//        return replace ?? tmp;
//    });
//    var epName = "";
//    var sEepNum = sEep.TrimStart('S').Split('E');
//    var cleanFilename = filename.TrimEnd('.').Replace('.', ' ');
//    var searchResults = tmdb.SearchTvShowAsync(cleanFilename).Result;
//    foreach (var item in searchResults.Results)
//    {
//        if (!item.Name.ToLower().Equals(cleanFilename.ToLower())) continue;
//        var uri =
//            $"https://api.themoviedb.org/3/tv/{item.Id}/season/{sEepNum[0]}/episode/{sEepNum[1]}?api_key={tmdBapikey}";
//        var client = new RestClient(uri);
//        var request = new RestRequest { Method = Method.GET };
//        request.AddHeader("Accept", "application/json");
//        request.Parameters.Clear();
//        var response = client.Execute(request);
//        var deserializeObjectJson = JsonConvert.DeserializeObject(response.Content);
//        var token = JObject.Parse(deserializeObjectJson.ToString());
//        var result = token.SelectToken("name").Value<string>();
//        var delimitersStrings = new[] { "-", "_", "+", "." };
//        var isDelimiter = delimitersStrings.Any(delimit => result.Contains(delimit));

//        epName = isDelimiter ? result.Replace(" ", string.Empty) : result.Replace(" ", ".");
//        break;
//    }
//    newName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(filename) + "." + sEep + epName + '.' + format + startwith + group + ext;

//}