using System.Collections.Generic;

namespace AD.Tools
{
    public class BoolArray : List<bool>
    {
        public bool AndResult
        {
            get
            {
                foreach (var b in this)
                    if (!b)
                        return false;

                return true;
            }
        }

        public bool OrResult
        {
            get
            {
                foreach (var b in this)
                    if (b)
                        return true;

                return false;
            }
        }
    }
}