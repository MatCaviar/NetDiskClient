using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo
{
    class NetDisk
    {
        public string fileid;
        private string createtime;
        private string name;

        private string bucket;
        private string fileextname;
        private string recordtype;
        private string size;
        private string record_id;

        public string Fileid
        {
            get
            {
                return fileid;
            }

            set
            {
                fileid = value;
            }
        }

        public string Createtime
        {
            get
            {
                return createtime;
            }

            set
            {
                createtime = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string Bucket
        {
            get
            {
                return bucket;
            }

            set
            {
                bucket = value;
            }
        }

        public string Fileextname
        {
            get
            {
                return fileextname;
            }

            set
            {
                fileextname = value;
            }
        }

        public string Recordtype
        {
            get
            {
                return recordtype;
            }

            set
            {
                recordtype = value;
            }
        }

        public string Size
        {
            get
            {
                return size;
            }

            set
            {
                size = value;
            }
        }

        public string Record_id
        {
            get
            {
                return record_id;
            }

            set
            {
                record_id = value;
            }
        }
    }
}
