using System;

namespace AvaloniaGUI {
    public class Data {
        public string Field0 { get; set; } = "";
        public string Field1 { get; set; } = "";
        public string Field2 { get; set; } = "";
        public string Field3 { get; set; } = "";
        public string Field4 { get; set; } = "";
        public string Field5 { get; set; } = "";
        public string Field6 { get; set; } = "";
        public string Field7 { get; set; } = "";

        public Data() { }

        public Data(Data another) {
            for (int c = 0; c < 8; c++) {
                this[c] = another[c];
            }
        }

        public override string ToString() {
            return $"{Field0} {Field1} {Field2} {Field3} {Field4} {Field5} {Field6} {Field7}";
        }

        public string this[int index] {
            get {
                switch (index) {
                    case 0: return Field0;
                    case 1: return Field1;
                    case 2: return Field2;
                    case 3: return Field3;
                    case 4: return Field4;
                    case 5: return Field5;
                    case 6: return Field6;
                    case 7: return Field7;
                    default:
                        throw new IndexOutOfRangeException("Wrong index");
                }
            }
            set {
                switch (index) {
                    case 0: Field0 = value; break;
                    case 1: Field1 = value; break;
                    case 2: Field2 = value; break;
                    case 3: Field3 = value; break;
                    case 4: Field4 = value; break;
                    case 5: Field5 = value; break;
                    case 6: Field6 = value; break;
                    case 7: Field7 = value; break;
                    default:
                        throw new IndexOutOfRangeException("Wrong index");
                }
            }
        }
    }
}
