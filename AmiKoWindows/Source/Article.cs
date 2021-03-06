﻿/*
Copyright (c) ywesee GmbH

This file is part of AmiKo for Windows.

AmiKo for Windows is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program. If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace AmiKoWindows
{
    public class Article
    {
        public long? Id { get; set; }
        public long? CustomerId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string AtcCode { get; set; }
        public string Substances { get; set; }
        public string Regnrs { get; set; }
        public string AtcClass { get; set; }
        public string Therapy { get; set; }
        public string Application { get; set; }
        public string Indications { get; set; }
        public string PackInfo { get; set; }
        public string AddInfo { get; set; }
        public string SectionIds { get; set; }
        public string SectionTitles { get; set; }
        public string Content { get; set; }
        public string Packages { get; set; }
        public bool IsFavorite { get; set; }

        public List<TitleItem> ListOfSectionTitleItems()
        {
            List<TitleItem> listOfTitleItems = new List<TitleItem>();
            using (var id = ListOfSectionIds().GetEnumerator())
            using (var title = ListOfSectionTitles().GetEnumerator())
            {
                while (id.MoveNext() && title.MoveNext())
                {
                    if (title.Current?.Length > 0)
                    {
                        listOfTitleItems.Add(new TitleItem
                        {
                            Id = id.Current,
                            Title = title.Current
                        });
                    }
                }
            }
            return listOfTitleItems;
        }

        public List<string> ListOfSectionIds()
        {
            List<string> listOfSectionIds = SectionIds?.Split(',').ToList();
            return listOfSectionIds;
        }

        // NOTE:
        // https://github.com/zdavatz/amiko_csharp/issues/169
        public List<string> ListOfSectionTitles()
        {
            string[] sectionsTitles = Constants.SectionTitlesDE;
            if (Utilities.AppLanguage().Equals("fr"))
                sectionsTitles = Constants.SectionTitlesFR;

            List<string> listOfSectionTitles = SectionTitles?.Split(';').ToList();
            for (int i = 0; i < listOfSectionTitles.Count; i++)
            {
                string s = listOfSectionTitles[i];
                foreach (string t in sectionsTitles)
                {
                    string titleA = s.Replace(" ", "");
                    string titleB = Title.Replace(" ", "");

                    // Check first if title
                    if (titleA.Contains(titleB) || titleB.Contains(titleA)) {
                        var n = s.IndexOf("®");
                        if (n > -1) {
                            s = s.Substring(0, n + 1);
                            listOfSectionTitles[i] = s;
                        }
                        continue;
                    }
                    else if (s.Contains(t))
                    {
                        listOfSectionTitles[i] = t;
                        break;
                    }
                    else
                        listOfSectionTitles[i] = listOfSectionTitles[i].Replace(" / ", "");
                }        
            }
            return listOfSectionTitles;
        }

        public Dictionary<int, string> IndexToTitlesMap()
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();

            using (var iter1 = ListOfSectionIds().GetEnumerator())
            using (var iter2 = ListOfSectionTitles().GetEnumerator())
            {
                while (iter1.MoveNext() && iter2.MoveNext())
                {
                    string section_id = iter1.Current?.Replace("section", "").Replace("Section", "");
                    if (section_id.Length > 0)
                    {
                        string title = iter2.Current;
                        int id = Convert.ToInt32(section_id);
                        if (title.Length > 0)
                            dict[id] = title;
                    }
                }
            }

            return dict;
        }
    }
}
