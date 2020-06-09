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
        /// Contains information about PTM
        /// </summary>
        public string SequenceInformation { get; set; }
        /// <summary>
        /// Main dictionary will all maps: <key: Study condition#FixedCondition1, value: (fixedCond1, fixedCond2, fixedCond3, allFragmentIonsAllConditions)>
        /// </summary>
        public Dictionary<string, (string, string, string, List<(string, int, string, int, string, int)>)> DictMaps { get; set; }

        public List<string> AllFragmentationMethods { get; set; }
        public List<int> AllPrecursorChargeStates { get; set; }

        public List<string> AllActivationLevels { get; set; }
        public List<int> AllReplicates { get; set; }

        /// <summary>
        /// Method responsible for updating inverse amino acids positions (x, y and z series)
        /// </summary>
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
        }
    }
}
