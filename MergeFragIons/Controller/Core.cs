using Ionic.Zip;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDFragMapper.Controller
{
    [ProtoContract]
    public class Core
    {
        /// <summary>
        /// List of Fragment Ions: FragmentationMethod: UVPD, EThcD, CID, HCD, SID, ECD, ETD; PrecursorChargeState, IonType: A,B,C,X,Y,Z, Aminoacid Position, Activation Level, Replicate, Intensity
        /// </summary>
        [ProtoMember(1)]
        public List<(string, int, string, int, string, int, double)> FragmentIons { get; set; }
        [ProtoMember(2)]
        public string ProteinSequence { get; set; }
        /// <summary>
        /// Contains information about PTM
        /// </summary>
        [ProtoMember(3)]
        public string SequenceInformation { get; set; }
        /// <summary>
        /// Main dictionary will all maps: <key: Study condition#FixedCondition1, value: (fixedCond1, fixedCond2, fixedCond3, allFragmentIonsAllConditions)>
        /// </summary>
        [ProtoMember(4)]
        public Dictionary<string, (string, string, string, List<(string, int, string, int, string, int, double)>)> DictMaps { get; set; }
        [ProtoMember(5)]
        public bool Has_And_LocalNormalization { get; set; }
        [ProtoMember(6)]
        public bool GlobalNormalization { get; set; }
        [ProtoMember(7)]
        public bool HasMergeMaps { get; set; }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public Core() { }

        /// <summary>
        /// Method responsible for updating inverse amino acids positions (x, y and z series)
        /// </summary>
        public void ProcessFragIons()
        {
            int proteinLength = ProteinSequence.Length;
            List<(string, int, string, int, string, int, double)> fragmentIons = FragmentIons.Where(a => a.Item3.Equals("X") || a.Item3.Equals("Y") || a.Item3.Equals("Z")).ToList();

            for (int i = 0; i < fragmentIons.Count; i++)
            {
                fragmentIons[i] = (fragmentIons[i].Item1, fragmentIons[i].Item2, fragmentIons[i].Item3, proteinLength - fragmentIons[i].Item4 + 1, fragmentIons[i].Item5, fragmentIons[i].Item6, fragmentIons[i].Item7);
            }

            FragmentIons.RemoveAll(a => a.Item3.Equals("X") || a.Item3.Equals("Y") || a.Item3.Equals("Z"));
            FragmentIons.AddRange(fragmentIons);
        }

        public List<(string, int, string, int, string, int, double)> MatchFragmentIonsAndIntensities(List<(string, int, string, int, string, int, double, string)> FragIonsWithFileName,
            List<(string, double, double)> Intensities)
        {
            /// string,int,string,int -> FragmentationMethod: UVPD, EThcD, CID, HCD, SID, ECD, ETD; PrecursorChargeState, IonType: A,B,C,X,Y,Z, Aminoacid Position, Activation Level, Replicate, Intensity
            List<(string, int, string, int, string, int, double)> FragIonsWithIntensities = new List<(string, int, string, int, string, int, double)>();
            /// string,int,string,int -> FragmentationMethod: UVPD, EThcD, CID, HCD, SID, ECD, ETD; PrecursorChargeState, IonType: A,B,C,X,Y,Z, Aminoacid Position, Activation Level, Replicate, Observed Mass, IntensityFile
            foreach ((string, int, string, int, string, int, double, string) fragment in FragIonsWithFileName)
            {
                List<(string, double, double)> currentIntesities = Intensities.Where(item => item.Item1.Equals(fragment.Item8)).ToList();
                double MH = fragment.Item7 + Utils.Util.HYDROGENMASS;

                /// string,double,double -> Intensity file, monoisotopic mass, sum intensity
                (string, double, double) intensity = currentIntesities.Where(item => Math.Abs(Utils.Util.PPM(item.Item2, MH)) < 5).FirstOrDefault();
                if (!intensity.Equals(default(ValueTuple<int, double, double>)))
                    FragIonsWithIntensities.Add((fragment.Item1, fragment.Item2, fragment.Item3, fragment.Item4, fragment.Item5, fragment.Item6, intensity.Item3));
                else
                    FragIonsWithIntensities.Add((fragment.Item1, fragment.Item2, fragment.Item3, fragment.Item4, fragment.Item5, fragment.Item6, 0));
            }

            return FragIonsWithIntensities;
        }

        public void SerializeResults(string fileName)
        {
            MemoryStream fileToCompress = new MemoryStream();
            Serializer.SerializeWithLengthPrefix(fileToCompress, this, PrefixStyle.Base128, 1);

            fileToCompress.Seek(0, SeekOrigin.Begin);   // <-- must do this after writing the stream!

            using (ZipFile zipFile = new ZipFile())
            {
                zipFile.Password = "TDFr@gM@pp3r!";
                zipFile.AddEntry("FileCompressed", fileToCompress);
                zipFile.Save(fileName);
            }
        }

        public Core DeserializeResults(string fileName)
        {
            using (var ms = new MemoryStream())
            {
                using (ZipFile zip = ZipFile.Read(fileName))
                {
                    ZipEntry entry = zip["FileCompressed"];
                    entry.ExtractWithPassword(ms, "TDFr@gM@pp3r!");// extract uncompressed content into a memorystream 

                    ms.Seek(0, SeekOrigin.Begin); // <-- must do this after writing the stream!

                    List<Core> toDeserialize = Serializer.DeserializeItems<Core>(ms, PrefixStyle.Base128, 1).ToList();
                    return toDeserialize[0];
                }
            }
        }
    }
}
