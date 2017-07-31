using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

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

        private void NewVersion(bool isLoad)
        {
            var downloadUrl = @"";
            Version newVersion = null;
            var xmlUrl = @"https://onedrive.live.com/download?cid=D9DE3B3ACC374428&resid=D9DE3B3ACC374428%217999&authkey=ADJwQu1VOTfAOVg";
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
                    $@"{appName} v.{newVersion} is out!{Environment.NewLine}Would You Like To Donwload It?", @"New Version is avlibale", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    System.Diagnostics.Process.Start(downloadUrl);
            }
            else
            {
                if (!isLoad)
                    MessageBox.Show(@"You Are Running The Last Version.", @"No New Updates");
            }
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

        //private bool _load = true;
        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //_load = true;
            listView.Items.Clear();
            var fs = new FolderSelectDialog();
            var result = fs.ShowDialog();
            if (!result) return;
            if (!Directory.Exists(fs.FileName)) return;
            var allfiles = Filtered_List(GetFiles(fs.FileName, "*.*"));
            foreach (var name in allfiles)
                ProccessName(name);
            //All_checkBox.Checked = true;
            //_load = false;

        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //_load = true;
            var fs = new OpenFileDialog
            {
                Title = @"Pick Some Files To ProccessName",
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
                ProccessName(name);
            //_load = false;
        }

        private static bool TestWord(string file, Regex filter)
        {
            return filter.IsMatch(file);
        }

        private static string SercheMatch(string input, string pattern)
        {
            var tmp = @"";
            var index = new List<int>();
            foreach (Match match in Regex.Matches(input, pattern))
            {
                tmp = match.Value;
                index.Add(input.IndexOf(tmp, StringComparison.Ordinal));
            }
            if (index.Count > 1)
                return input.Substring(index[0], index[index.Count - 1] + tmp.Length + 1);
            return tmp;
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

        private static string ProccessNewName(string name)
        {
            var original = Path.GetFileName(name);
            var tmpFile = Path.GetFileNameWithoutExtension(name);
            var ext = Path.GetExtension(name);
            var newName = "";
            if (tmpFile != null)
            {
                string format;
                string group;
                tmpFile = tmpFile.EndsWith(".") ? tmpFile.Substring(0, tmpFile.Length - 1) : tmpFile;
                
                if (Matches.SpRegex.IsMatch(tmpFile)) newName = original;
                else if (Matches.SXregex.IsMatch(tmpFile))
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

                    Matches.SXregex = new Regex(@"([0-9]{1,2}[xX][0-9]{1,2}).*");
                    var filename = Matches.SXregex.Replace(tmpFile, string.Empty);
                    if (filename.EndsWith(".")) filename = filename.Substring(0, filename.Length - 1);
                    tmpFile = tmpFile.Substring(filename.Length + seSize + epSize);
                    tmpFile = tmpFile.StartsWith(".") ? tmpFile.Substring(1) : tmpFile;
                    format = SercheMatch(tmpFile, Matches.FormatRegex.ToString());
                    var fDict = Matches.FormatList.ToDictionary(x => x, StringComparer.CurrentCultureIgnoreCase);
                    format = Regex.Replace(format.ToUpper(), @"\b.+?\b", x =>
                    {
                        fDict.TryGetValue(x.Value, out string replace);
                        return replace ?? x.Value;
                    });
                    var fMatch = Regex.Match(format, "(X|H)?(264|265)");
                    var mc = fMatch.Value;
                    format = Regex.Replace(format, mc, mc.ToLower());
                    tmpFile = tmpFile.Substring(format.Length);
                    var startwith = @"";
                    if (tmpFile.StartsWith("-"))
                    {
                        startwith = @"-";
                        tmpFile = tmpFile.Substring(1);
                    }
                    else if (tmpFile.StartsWith("."))
                    {
                        startwith = @".";
                        tmpFile = tmpFile.Substring(1);
                    }
                    group = SercheMatch(tmpFile, Matches.GroupRegex.ToString());
                    var gDict = Matches.GroupList.ToDictionary(x => x, StringComparer.CurrentCultureIgnoreCase);
                    group = Regex.Replace(group, @"[^\>]*", x =>
                    {
                        var tmp = x.Value.StartsWith("-") ? x.Value.Substring(1) : x.Value;
                        string replace;
                        gDict.TryGetValue(tmp, out replace);
                        return replace ?? tmp;
                    });
                    newName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(filename) + "." + sEep + "." + format + startwith + group + ext;

                }
                else if (Matches.ThreeRegex.IsMatch(tmpFile))
                {
                    const string yPattern = @"^(19|20)[0-9][0-9]";
                    const string s3Pattern = @"\b\d{3}\b";
                    const string s4Pattern = @"\b\d{4}\b";
                    var tmpRegex = new Regex(@"([0-9]{1,2}[0-9]{1,2}).*");
                    var filename = tmpRegex.Replace(tmpFile, string.Empty);
                    if (filename.EndsWith(".")) filename = filename.Substring(0, filename.Length - 1);
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
                        sEep = sEep.Length > 3 ? "S" + sEep.Substring(0, 2) + "E" + sEep.Substring(2) + "." : "S0" + sEep.Substring(0, 1) + "E" + sEep.Substring(1) + ".";
                    tmpFile = tmpFile.Substring(sEepSize);
                    tmpFile = tmpFile.StartsWith(".") ? tmpFile.Substring(1) : tmpFile;
                    format = SercheMatch(tmpFile, Matches.FormatRegex.ToString());
                    var fDict = Matches.FormatList.ToDictionary(x => x, StringComparer.CurrentCultureIgnoreCase);
                    format = Regex.Replace(format.ToUpper(), @"\b.+?\b", x =>
                    {
                        string replace;
                        fDict.TryGetValue(x.Value, out replace);
                        return replace ?? x.Value;
                    });
                    var fMatch = Regex.Match(format, "(X|H)?(264|265)");
                    var mc = fMatch.Value;
                    format = Regex.Replace(format, mc, mc.ToLower());
                    tmpFile = tmpFile.Substring(format.Length);
                    var startwith = @"";
                    if (tmpFile.StartsWith("-"))
                    {
                        startwith = @"-";
                        tmpFile = tmpFile.Substring(1);
                    }
                    else if (tmpFile.StartsWith("."))
                    {
                        startwith = @".";
                        tmpFile = tmpFile.Substring(1);
                    }
                    group = SercheMatch(tmpFile, Matches.GroupRegex.ToString());
                    var gDict = Matches.GroupList.ToDictionary(x => x, StringComparer.CurrentCultureIgnoreCase);
                    group = Regex.Replace(group, @"[^\>]*", x =>
                    {
                        var tmp = x.Value.StartsWith("-") ? x.Value.Substring(1) : x.Value;
                        string replace;
                        gDict.TryGetValue(tmp, out replace);
                        return replace ?? tmp;
                    });
                    newName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(filename) + "." + year + sEep + format + startwith + group + ext;
                }
                else if (Matches.FourRegex.IsMatch(tmpFile))
                {
                    const string yPattern = @"^(19|20)[0-9][0-9]";
                    const string s3Pattern = @"\b\d{3}\b";
                    const string s4Pattern = @"\b\d{4}\b";
                    var tmpRegex = new Regex("(19|20)[0-9][0-9].*");
                    var filename = tmpRegex.Replace(tmpFile, string.Empty);
                    if (filename.EndsWith(".")) filename = filename.Substring(0, filename.Length - 1);
                    tmpFile = tmpFile.Substring(filename.Length + 1).ToLower();
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
                    format = SercheMatch(tmpFile, Matches.FormatRegex.ToString());
                    var fDict = Matches.FormatList.ToDictionary(x => x, StringComparer.CurrentCultureIgnoreCase);
                    format = Regex.Replace(format.ToUpper(), @"\b.+?\b", x =>
                    {
                        string replace;
                        fDict.TryGetValue(x.Value, out replace);
                        return replace ?? x.Value;
                    });
                    var fMatch = Regex.Match(format, "(X|H)?(264|265)");
                    var mc = fMatch.Value;
                    format = Regex.Replace(format, mc, mc.ToLower());
                    tmpFile = tmpFile.Substring(format.Length);
                    var startwith = @"";
                    if (tmpFile.StartsWith("-"))
                    {
                        startwith = @"-";
                        tmpFile = tmpFile.Substring(1);
                    }
                    else if (tmpFile.StartsWith("."))
                    {
                        startwith = @".";
                        tmpFile = tmpFile.Substring(1);
                    }
                    group = SercheMatch(tmpFile, Matches.GroupRegex.ToString());
                    var gDict = Matches.GroupList.ToDictionary(x => x, StringComparer.CurrentCultureIgnoreCase);
                    group = Regex.Replace(group, @"[^\>]*", x =>
                    {
                        var tmp = x.Value.StartsWith("-") ? x.Value.Substring(1) : x.Value;
                        string replace;
                        gDict.TryGetValue(tmp, out replace);
                        return replace ?? tmp;
                    });

                    newName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(filename) + "." + year + sEep + format + startwith + group + ext;

                }
                else if (Matches.MorethenfourRegex.IsMatch(tmpFile))
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
                        sEep = "S0" + sEep.Substring(0, 1) + "E" + sEep.Substring(1) + ".";
                    tmpFile = tmpFile.Substring(sEepSize);
                    tmpFile = tmpFile.StartsWith(".") ? tmpFile.Substring(1) : tmpFile;
                    format = SercheMatch(tmpFile, Matches.FormatRegex.ToString());
                    var fDict = Matches.FormatList.ToDictionary(x => x, StringComparer.CurrentCultureIgnoreCase);
                    format = Regex.Replace(format.ToUpper(), @"\b.+?\b", x =>
                    {
                        string replace;
                        fDict.TryGetValue(x.Value, out replace);
                        return replace ?? x.Value;
                    });
                    var fMatch = Regex.Match(format, "(X|H)?(264|265)");
                    var mc = fMatch.Value;
                    format = Regex.Replace(format, mc, mc.ToLower());
                    tmpFile = tmpFile.Substring(format.Length);
                    var startwith = @"";
                    if (tmpFile.StartsWith("-"))
                    {
                        startwith = @"-";
                        tmpFile = tmpFile.Substring(1);
                    }
                    else if (tmpFile.StartsWith("."))
                    {
                        startwith = @".";
                        tmpFile = tmpFile.Substring(1);
                    }
                    group = SercheMatch(tmpFile, Matches.GroupRegex.ToString());
                    var gDict = Matches.GroupList.ToDictionary(x => x, StringComparer.CurrentCultureIgnoreCase);
                    group = Regex.Replace(group, @"[^\>]*", x =>
                    {
                        var tmp = x.Value.StartsWith("-") ? x.Value.Substring(1) : x.Value;
                        string replace;
                        gDict.TryGetValue(tmp, out replace);
                        return replace ?? tmp;
                    });

                    newName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(filename) + "." + sEep + format + startwith + group + ext;

                }
                else
                {
                    var filename = "";
                    const string sEepPattern = @"[s][0-9]{2}[e][0-9]{2}";
                    var sEepMatch = Regex.Match(tmpFile, sEepPattern);
                    var sEep = sEepMatch.Value;
                    var sEepSize = sEep.Length;
                    if (sEepSize != 0)
                    {
                        tmpFile = tmpFile.Replace(sEep + ".", string.Empty);
                        sEep = sEep.ToUpper() + ".";
                    }
                    var tmpFileSplit = tmpFile.Split('.');
                    foreach (var item in tmpFileSplit)
                    {
                        if (!TestWord(item, Matches.FormatRegex)) filename += item + ".";
                        else break;
                    }
                    filename = filename.Substring(0, filename.Length - 1);
                    tmpFile = tmpFile.Substring(filename.Length + 1);
                    format = SercheMatch(tmpFile, Matches.FormatRegex.ToString());
                    format = format.Substring(0, format.Length - 1);
                    tmpFile = tmpFile.StartsWith("-") ? tmpFile.Substring(1) : tmpFile;
                    var fDict = Matches.FormatList.ToDictionary(x => x, StringComparer.CurrentCultureIgnoreCase);
                    format = Regex.Replace(format.ToUpper(), @"\b.+?\b", x =>
                    {
                        string replace;
                        fDict.TryGetValue(x.Value, out replace);
                        return replace ?? x.Value;
                    });
                    var fMatch = Regex.Match(format, "(X|H)?(264|265)");
                    var mc = fMatch.Value;
                    format = Regex.Replace(format, mc, mc.ToLower());
                    tmpFile = tmpFile.Substring(format.Length);
                    var startwith = @"";
                    if (tmpFile.StartsWith("-"))
                    {
                        startwith = @"-";
                        tmpFile = tmpFile.Substring(1);
                    }
                    else if (tmpFile.StartsWith("."))
                    {
                        startwith = @".";
                        tmpFile = tmpFile.Substring(1);
                    }


                    group = SercheMatch(tmpFile, Matches.GroupRegex.ToString());
                    var gDict = Matches.GroupList.ToDictionary(x => x, StringComparer.CurrentCultureIgnoreCase);
                    group = Regex.Replace(group, @"[^\>]*", x =>
                    {
                        var tmp = x.Value.StartsWith("-") ? x.Value.Substring(1) : x.Value;
                        string replace;
                        gDict.TryGetValue(tmp, out replace);
                        return replace ?? tmp;
                    });
                    newName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(filename) + "." + sEep + format + startwith + group + ext;
                }
            }
            if (newName != null)
            {
                var match = Regex.Match(newName, "(X|H)?(264|265)");
                var ma = match.Value;
                newName = Regex.Replace(newName, ma, ma.ToLower());
                 if (newName.Contains("HDTV") && !newName.Contains("XviD") && !Regex.IsMatch(newName,"([xX]|[hH])?(264|265)"))
                newName = newName.Replace("HDTV", "HDTV.x264");
            }
           
            return newName;
        }
        private void ProccessName(string name)
        {
            var i = listView.Items.Count;
            var original = Path.GetFileName(name);
            var directoryName = Path.GetDirectoryName(name);
            var index = ++i;
            var newName = ProccessNewName(name);
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

            listView.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void VideoRenamer_Shown(object sender, EventArgs e)
        {
            void Action()
            {
                NewVersion(true);
            }

            var thread = new Thread(Action) { IsBackground = true };
            thread.Start();
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewVersion(false);
        }

        private void All_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            listView.ItemChecked -= listView_ItemChecked;
            if (All_checkBox.Checked)
            {
                for (var i = 0; i < listView.Items.Count; i++)
                {
                    listView.Items[i].Checked = true;
                    listView.Items[i].SubItems[3].Text = ProccessNewName(listView.Items[i].SubItems[2].Text);
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
                MessageBox.Show(@"Done But with Errors." + Environment.NewLine +  @"The Following Couldn't Be Renamed:" + Environment.NewLine + errors, @"Done");
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
                        item.SubItems[3].Text = ProccessNewName(item.SubItems[2].Text);
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
    }
}
