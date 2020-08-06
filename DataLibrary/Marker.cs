namespace DataLibrary
{
    public class Marker
    {
        public string name;

        public int id;

        public Vector3 pos;

        public Vector4 color;

        public Marker(string name, int id, float posx, float posy, float posz,
            float colx, float coly, float colz, float colw)
        {
            this.name = name;
            this.id = id;
            this.pos = new Vector3(posx, posy, posz);
            this.color = new Vector4(colx, coly, colz, colw);
        }


        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            try
            {
                Marker other = (Marker)obj;
                return other.color == this.color
                    && other.id == this.id
                    && other.name == this.name
                    && other.pos == this.pos;
            }
            catch
            {
                return false;
            }
        }
    }
}
