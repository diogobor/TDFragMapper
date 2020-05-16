using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeFragIons.Controller
{
    public class Core
    {
        /// <summary>
        /// List of Fragment Ions: FragmentationMethod: UVPD, EThcD, CID, HCD, SID, ECD, ETD; PrecursorChargeState, IonType: A,B,C,X,Y,Z, Aminoacid Position, Activation Level, Replicate
        /// </summary>
        public List<(string, int, string, int, string, int)> FragmentIons { get; set; }
        public string ProteinSequence { get; set; }
        /// <summary>
        /// Main dictionary will all maps: <key: Study condition#FixedCondition1, value: (fixedCond1, fixedCond2, fixedCond3, allFragmentIonsAllConditions)>
        /// </summary>
        public Dictionary<string, (string, string, string, List<(string, int, string, int, string, int)>)> DictMaps { get; set; }

        public List<string> AllFragmentationMethods { get; set; }
        public List<int> AllPrecursorChargeStates { get; set; }

        public List<string> AllActivationLevels { get; set; }
        public List<int> AllReplicates { get; set; }

        public void ProcessFragIons()
        {
            int proteinLength = ProteinSequence.Length;
            List<(string, int, string, int, string, int)> fragmentIons = FragmentIons.Where(a => a.Item3.Equals("X") || a.Item3.Equals("Y") || a.Item3.Equals("Z")).ToList();

            for (int i = 0; i < fragmentIons.Count; i++)
            {
                fragmentIons[i] = (fragmentIons[i].Item1, fragmentIons[i].Item2, fragmentIons[i].Item3, proteinLength - fragmentIons[i].Item4 + 1, fragmentIons[i].Item5, fragmentIons[i].Item6);
            }

            FragmentIons.RemoveAll(a => a.Item3.Equals("X") || a.Item3.Equals("Y") || a.Item3.Equals("Z"));
            FragmentIons.AddRange(fragmentIons);
            this.SetDictionaryFragMethPrecursorChargeStates();
        }

        private void SetDictionaryFragMethPrecursorChargeStates()
        {
            //List<int> allPrecursorChargeStates = (from fragIon in FragmentIons
            //                                      select fragIon.Item2).Distinct().ToList();

            //List<string> fragMethods = (from fi in FragmentIons
            //                            select fi.Item1).Distinct().ToList();
            ////Sort List fragMet-> UVPD, EThcD, CID, HCD, SID, ECD, ETD
            //List<string> tmpListFragMeth = new List<string>();
            //if (fragMethods.Contains("UVPD"))
            //    tmpListFragMeth.Add("UVPD");
            //if ((fragMethods.Contains("ETHCD") || fragMethods.Contains("ethcd") || fragMethods.Contains("EThcD")))
            //    tmpListFragMeth.Add("ETHCD");
            //if (fragMethods.Contains("CID"))
            //    tmpListFragMeth.Add("CID");
            //if (fragMethods.Contains("HCD"))
            //    tmpListFragMeth.Add("HCD");
            //if (fragMethods.Contains("SID"))
            //    tmpListFragMeth.Add("SID");
            //if (fragMethods.Contains("ECD"))
            //    tmpListFragMeth.Add("ECD");
            //if (fragMethods.Contains("ETD"))
            //    tmpListFragMeth.Add("ETD");
            //fragMethods = tmpListFragMeth;

            //FragMethodsWithPrecursorChargeStates = new Dictionary<string, List<int>>();
            //foreach (string fragMethod in fragMethods)
            //    FragMethodsWithPrecursorChargeStates.Add(fragMethod, allPrecursorChargeStates);

            //AllFragmentationMethods = fragMethods;
            //AllPrecursorChargeStates = allPrecursorChargeStates;
            //AllActivationLevels = (from fragIon in FragmentIons
            //                       select fragIon.Item5).Distinct().ToList();
            //AllReplicates = (from fragIon in FragmentIons
            //                 select fragIon.Item6).Distinct().ToList();
        }
    }
}
