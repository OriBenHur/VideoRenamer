using BrightIdeasSoftware;
using Newtonsoft.Json;
using RestSharp;
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
using TMDbLib.Client;
using TMDbLib.Objects.TvShows;


namespace VideoRenamer
{
    public partial class VideoRenamer : Form
    {
        private List<VideoItem> _items = new List<VideoItem>();
        private class VideoItem
        {
            public string PlaceHolder;
            public int Index;
            public string OldName;
            public string OutputName;
            public string Dir;
        }

        public VideoRenamer()
        {
            InitializeComponent();

        }



        //private void VideoRenamer_Load(object sender, EventArgs e)
        //{
        //    var width = listView.Size.Width - 52;
        //    listView.View = View.Details;
        //    listView.FullRowSelect = true;
        //    listView.Columns.Add("", 24);
        //    listView.Columns.Add("#", 24);
        //    listView.Columns.Add("Original Name", width / 2);
        //    listView.Columns.Add("New Name", width / 2);
        //    listView.CheckBoxes = true;
        //    // Ensure that the view is set to show details.
        //    listView.View = View.Details;
        //    // Loop through and size each column header to fit the column header text.
        //    foreach (ColumnHeader ch in this.listView.Columns)
        //    {
        //        ch.Width = -2;
        //    }
        //}

        //private void SizeLastColumn(ListView listview)
        //{
        //    var width = listView.Size.Width - 52;
        //    listview.Columns[2].Width = width / 2;
        //    listview.Columns[3].Width = width / 2;
        //}

        private static IEnumerable<string> Filtered_List(IList<string> list)
        {
            return (from name in list let extension = Path.GetExtension(name) where extension != null let ext = extension.ToLower() where ext.Equals(".mp4") || ext.Equals(".avi") || ext.Equals(".mkv") || ext.Equals(".srt") select name).ToList();
        }

        private static Dictionary<string, string> _matches = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private static Dictionary<string, string> _mark = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly List<string> _list = new List<string>();

        //private bool _load = true;
        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //_load = true;
            olv1.ClearObjects();
            _matches.Clear();
            _items.Clear();
            _mark.Clear();
            var fs = new FolderSelectDialog();
            var result = fs.ShowDialog();
            if (!result)
            {
                return;
            }

            if (!Directory.Exists(fs.FileName))
            {
                return;
            }

            var allfiles = Filtered_List(GetFiles(fs.FileName, "*.*"));
            foreach (var name in allfiles)
            {
                if (!_matches.ContainsKey(Path.GetFileName(name)))
                {
                    ProcessName(name);
                }
            }
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
            if (result != DialogResult.OK)
            {
                return;
            }

            var fileName = fs.FileNames;
            foreach (var name in fileName)
            {
                if (!_matches.ContainsKey(Path.GetFileName(name)))
                {
                    ProcessName(name);
                }
            }
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
                foreach (var directory in Directory.GetDirectories(path))
                {
                    files.AddRange(GetFiles(directory, pattern));
                }
            }
            catch
            {
                Console.WriteLine(@"Opps!");
            }

            return files;
        }

        private void ReName(string original, string output)
        {
            try
            {
                File.Move(original, output);

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
                var j = 0;
                var title = GetTitle(tmpFile);
                j += title.Length;
                var year = GetYear(tmpFile.Substring(j));
                j += year.Length;
                var sEepObj = GetSeEp(tmpFile.Substring(j));
                var sEep = (string)sEepObj[0];
                j += (int)sEepObj[1];
                var format = GetFormat(tmpFile.Substring(j));
                j += format.Length;
                var group = GetGroup(tmpFile.Substring(j));
                var startWith = GetStartWite(tmpFile.Substring(j));
                var epName = GetEpName(title.TrimEnd('.').Replace(".", " "), tmdb, sEep, year);
                if (original != null && !string.IsNullOrEmpty(epName) && original.ToLower().Contains(epName.ToLower()))
                {
                    newName = original;
                }
                else
                {
                    newName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title) + year + sEep + epName + format + startWith + group + ext;
                }
            }

            var match = Regex.Match(newName, "(X|H)?(264|265)");
            var ma = match.Value;
            newName = Regex.Replace(newName, ma, ma.ToLower());
            if (newName.Contains("HDTV") && !newName.Contains("XviD") && !Regex.IsMatch(newName, "([xX]|[hH])?(264|265)"))
            {
                newName = newName.Replace("HDTV", "HDTV.x264");
            }

            if (original != null && !_matches.ContainsKey(original))
            {
                _matches.Add(original, newName);
            }

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
            if (_mark.ContainsValue(title))
            {
                var tmpYear = _mark.FirstOrDefault(z => z.Value.ToLower().Equals(title.ToLower())).Key.Split('-');
                year = tmpYear[0];
            }

            var potential = new List<dynamic>();
            var sEepNum = seEp.ToUpper().TrimEnd('.').TrimStart('S').Split('E');
            if (string.IsNullOrEmpty(seEp))
            {
                return "";
            }

            var tvUri = $@"https://api.themoviedb.org/3/search/tv?api_key={tmdb.ApiKey}&language=en-US&query={title}&page=1&first_air_date_year={year}";
            var response = GetJson(tvUri);
            foreach (var result in response.results)
            {
                if (((string)result.name).ToLower().Equals(title.ToLower()))
                {
                    potential.Add(result);
                }
            }

            if (potential.Count == 0)
            {
                return "";
            }


            if (potential.Count == 1 || AndroidStyleToggleSwitch.Checked)
            {
                var uri = $@"https://api.themoviedb.org/3/tv/{potential[0].id}/season/{sEepNum[0]}/episode/{sEepNum[1]}?api_key={tmdb.ApiKey}";
                var tvJson = GetJson(uri);
                var delimitersStrings = new[] { "-", "_", "+" };

                var CleanName = ((string)tvJson.name).TrimEnd('.');
                var isDelimiter = delimitersStrings.Any(delimit => ((string)tvJson.name).Contains(delimit));
                return isDelimiter ? CleanName.Replace(" ", string.Empty) + "." : CleanName.Replace(" ", ".") + ".";

            }



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
                if (!imdbId.Equals(msg.MyId))
                {
                    continue;
                }

                var uri = $@"https://api.themoviedb.org/3/tv/{potential[0].id}/season/{sEepNum[0]}/episode/{sEepNum[1]}?api_key={tmdb.ApiKey}";
                var tvJson = GetJson(uri);
                var delimitersStrings = new[] { "-", "_", "+" };
                var CleanName = ((string)tvJson.name).TrimEnd('.');
                var isDelimiter = delimitersStrings.Any(delimit => ((string)tvJson.name).Contains(delimit));
                _mark.Add((string)selected.first_air_date, (string)selected.original_name);
                return isDelimiter ? CleanName.Replace(" ", string.Empty) + "." : CleanName.Replace(" ", ".") + ".";
            }

            return "";

        }

        private static string GetGroup(string tmpFile)
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

        private static string GetFormat(string tmpFile)
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

        private static string GetStartWite(string tmpFile)
        {
            if (tmpFile.StartsWith("-"))
            {
                return @"-";

            }

            return tmpFile.StartsWith(".") ? @"." : "";
        }

        private static object[] GetSeEp(string tmpFile)
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
                                {
                                    return new object[] { sEep.ToUpper() + ".", 7 };
                                }
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
                                {
                                    //return se + ep + ".";
                                    return new object[] { se + ep + ".", sRegex.Value.Length + _epRegex.Value.Length };
                                }
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
                                if (!sRegex.Success)
                                {
                                    sRegex = Regex.Match(tmpFile, s4Pattern);
                                }

                                var sEep = sRegex.Value.ToLower();
                                var sEepSize = sEep.Length;
                                if (sEepSize != 0)
                                {
                                    return sEepSize > 3
                                        ? new object[] { "S" + sEep.Substring(0, 2) + "E" + sEep.Substring(2) + ".", 5 }
                                        : new object[] { "S0" + sEep.Substring(0, 1) + "E" + sEep.Substring(1) + ".", 4 };
                                }
                            }
                            break;

                        case "other":
                            {
                                const string m4Pattern = @"\b\d{5,10}\b";
                                var tmpRegex = new Regex(@"\b\d{5,10}\b.*");
                                var filename = tmpRegex.Replace(tmpFile, string.Empty);
                                if (filename.EndsWith("."))
                                {
                                    filename = filename.Substring(0, filename.Length - 1);
                                }

                                tmpFile = tmpFile.Substring(filename.Length + 1).ToLower();
                                var sRegex = Regex.Match(tmpFile, m4Pattern);
                                var sEep = sRegex.Value.ToLower();
                                var sEepSize = sEep.Length;
                                if (sEepSize != 0)
                                {
                                    return new object[] { "S0" + sEep.Substring(0, 1) + "E" + sEep.Substring(1) + ".", 3 };
                                }
                            }
                            break;
                        default:
                            return new object[] { "", 0 };

                    }
                }
            }

            return new object[] { "", 0 };

        }

        private static string GetYear(string tmpFile)
        {
            var match = Regex.Match(tmpFile, Matches.YRegex);

            return match.Success ? match.Value + "." : "";
        }

        private static string GetTitle(string tmpFile)
        {
            var tmp = tmpFile;

            foreach (var match in Matches.seEp_dic)
            {
                if (!match.Value.IsMatch(tmpFile))
                {
                    continue;
                }
                tmp = match.Value.Replace(tmpFile, string.Empty);
                break;

            }
            return Regex.Replace(tmp, $"{Matches.YRegex}.*", string.Empty);
            //return Regex.Replace(tmpFile, $"{Matches.YRegex}.*", string.Empty);
        }

        private void ProcessName(string name)
        {
            var i = _items.Count;
            var original = Path.GetFileName(name);
            var directoryName = Path.GetDirectoryName(name);
            var row = ++i;
            if (name == null)
            {
                return;
            }

            var result = _items.Find(x => x.OldName.Contains(name));
            if (result == null)
            {
                var processNewName = ProcessNewName(name);
                olv1.ItemChecked -= olv1_ItemChecked;
                _items.Add(new VideoItem { Index = row, OldName = original, OutputName = processNewName, Dir = directoryName });
                olv1.ClearObjects();
                olv1.AddObjects(_items);
                olv1.AutoResizeColumns();
                olv1.ItemChecked += olv1_ItemChecked;
                if (!_items[row - 1].OldName.Equals(_items[row - 1].OutputName))
                {
                    olv1.CheckObject(_items[row - 1]);
                    olv1.UseCellFormatEvents = true;
                    olv1.FormatCell += (sender, args) =>
                    {
                        if (args.ColumnIndex == outputName.Index && args.RowIndex == row - 1)
                        {
                            args.SubItem.ForeColor = Color.Red;
                        }
                    };

                }
                else
                {
                    _items[row - 1].OutputName = "";
                }
                olv1.RefreshObject(_items);
                olv1.BuildList(true);
            }

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

        private void Rename_button_Click(object sender, EventArgs e)
        {
            _list.Clear();
            var errors = @"";
            foreach (OLVListItem item in olv1.CheckedItems)
            {
                var original = $@"{_items[item.Index].Dir}\{_items[item.Index].OldName}";
                var newName = $@"{_items[item.Index].Dir}\{_items[item.Index].OutputName}";
                ReName(original, newName);
                _items[item.Index].PlaceHolder = _items[item.Index].OldName;
                _items[item.Index].OldName = _items[item.Index].OutputName;
                _items[item.Index].OutputName = "";
                item.CheckState = CheckState.Unchecked;
            }
            olv1.RefreshObject(_items);
            olv1.BuildList(true);
            olv1.UncheckHeaderCheckBox(checkBox);
            if (_list.Count > 0)
            {
                errors = _list.Aggregate(errors, (current, err) => current + err + Environment.NewLine);
                MessageBox.Show(@"Done But with Errors." + Environment.NewLine + @"The Following Couldn't Be Renamed:" + Environment.NewLine + errors, @"Done");
            }

            else
            {
                MessageBox.Show(@"Done.", @"Done");
            }
        }


        private void CheckForUpdates_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var downloadUrl = @"";
            XElement change = null;
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
                    if (versionElement == null)
                    {
                        continue;
                    }

                    var urlelEment = dm.Element(@"url");
                    if (urlelEment == null)
                    {
                        continue;
                    }

                    newVersion = new Version(versionElement.Value);
                    downloadUrl = urlelEment.Value;
                    change = dm.Element(@"change_log");

                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

            if (appVersion.CompareTo(newVersion) < 0)
            {
                if (change == null)
                {
                    return;
                }

                change.Value = change.Value;
                var result = MessageBox.Show(
                    $@"{appName.Replace('_', ' ')} v.{newVersion} is out!{Environment.NewLine}{change.Value}",
                    @"New Version is available", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(downloadUrl);
                }
            }
            else
            {
                if (!(bool)e.Argument)
                {
                    MessageBox.Show(@"You Are Running The Last Version.", @"No New Updates");
                }
            }
        }


        private void olv1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (!e.Item.Checked)
            {
                _items[e.Item.Index].OutputName = "";
            }
            else
            {

                var key = _items[e.Item.Index].OldName;
                _items[e.Item.Index].OutputName = _matches[key];
                if (!_items[e.Item.Index].OldName.Equals(_items[e.Item.Index].OutputName))
                {
                    olv1.FormatCell += (obj, args) =>
                    {
                        if (args.ColumnIndex == outputName.Index && args.RowIndex == e.Item.Index)
                        {
                            args.SubItem.ForeColor = Color.Red;
                        }
                    };
                }
                else
                {
                    _items[e.Item.Index].OutputName = "";
                    olv1.UncheckObject(_items[e.Item.Index]);
                }
            }

            olv1.RefreshObject(_items);
            olv1.BuildList(true);
        }



        private void olv1_HeaderCheckBoxChanging(object sender, HeaderCheckBoxChangingEventArgs e)
        {
            var checkState = olv1.GetColumn(0).HeaderCheckState;
            olv1.ItemChecked -= olv1_ItemChecked;
            if (checkState == CheckState.Unchecked)
            {

                if (olv1.Items.Count < 1)
                {
                    e.NewCheckState = CheckState.Unchecked;
                }

                foreach (var item in _items)
                {
                    var key = _items[item.Index].PlaceHolder == "" ? _items[item.Index].OldName : _items[item.Index].PlaceHolder;
                    item.OutputName = _matches[key];
                    if (!item.OldName.Equals(item.OutputName))
                    {
                        olv1.FormatCell += (obj, args) =>
                        {
                            if (args.ColumnIndex == outputName.Index && args.RowIndex == item.Index - 1)
                            {
                                args.SubItem.ForeColor = Color.Red;
                            }
                        };

                    }
                    else
                    {
                        item.OutputName = "";
                        olv1.UncheckObject(_items[item.Index - 1]);
                        //olv1.FormatCell += (obj, args) =>
                        //{
                        //    if (args.ColumnIndex == outputName.Index && args.RowIndex == item.Index - 1)
                        //    {
                        //        args.SubItem.ForeColor = Color.Black;
                        //    }
                        //};
                    }
                }

            }
            else
            {
                foreach (var item in _items)
                {
                    item.OutputName = "";
                }
            }
            olv1.RefreshObject(_items);
            olv1.BuildList(true);
            olv1.ItemChecked += olv1_ItemChecked;


        }

        private void olv1_CellEditFinished(object sender, CellEditEventArgs e)
        {
            _items[e.SubItemIndex].OldName = _items[e.SubItemIndex].OutputName;
            olv1.RefreshObject(_items);
            olv1.BuildList(true);

        }

        //private void All_checkBox_CheckedChanged(object sender, EventArgs e)
        //{
        //    olv1.ItemChecked -= listView_ItemChecked;
        //    if (All_checkBox.Checked)
        //    {
        //        for (var i = 0; i < listView.Items.Count; i++)
        //        {
        //            listView.Items[i].Checked = true;
        //            //listView.Items[i].SubItems[3].Text = ProcessNewName(listView.Items[i].SubItems[2].Text);
        //            listView.Items[i].SubItems[3].Text = _matches[listView.Items[i].SubItems[2].Text];
        //            if (!listView.Items[i].SubItems[2].Text.Equals(listView.Items[i].SubItems[3].Text))
        //            {
        //                listView.Items[i].UseItemStyleForSubItems = false;
        //                listView.Items[i].SubItems[3].ForeColor = Color.Red;
        //            }
        //            else
        //            {
        //                listView.Items[i].UseItemStyleForSubItems = false;
        //                listView.Items[i].SubItems[3].ForeColor = Color.Black;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        for (var i = 0; i < listView.Items.Count; i++)
        //        {
        //            listView.Items[i].Checked = false;
        //            listView.Items[i].SubItems[3].Text = "";
        //        }
        //    }

        //    listView.ItemChecked += listView_ItemChecked;
        //}

    }
}