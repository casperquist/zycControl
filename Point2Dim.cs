using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZYCControl
{
    public class Point2Dim
    {
        public float x { get; set; }
        public float y { get; set; }

        public Point2Dim()
        {
        }

	    public Point2Dim(float a , float b)
        {
            this.x = a;
            this.y = b;
        }

        public void Clone(out Point2Dim p)
        {
            p = new Point2Dim();
            p.x = this.x;
            p.y = this.y;;
        }

	    public static Point2Dim operator+ (Point2Dim p, Point2Dim a)
        {
		    return new Point2Dim(p.x+a.x,p.y+a.y);
	    }

	    public static Point2Dim operator- (Point2Dim p, Point2Dim a)
        {
		    return new Point2Dim(p.x-a.x,p.y-a.y);
	    }

	    public static Point2Dim operator* (Point2Dim p, Point2Dim a)
        {
		    return new Point2Dim(p.x*a.x,p.y*a.y);
	    }

	    public static Point2Dim operator* (Point2Dim p, float a)
        {
		    return new Point2Dim(p.x*a,p.y*a);
	    }

	    public static Point2Dim operator/ (Point2Dim p, float a)
        {
		    return new Point2Dim(p.x/a,p.y/a);
	    }

        public static Point2Dim operator /(Point2Dim p, Point2Dim a)
        {
		    return new Point2Dim(p.x/a.x,p.y/a.y);
	    }

	    public float sum()
        {
		    return x+y;
	    }

	    public void Initial()
        {
		    this.x = 0;
		    this.y = 0;
	    }

	    public float magnitude()
        {
		    return x*x+y*y;
	    }

	    public float normVec()
        {
		    return (float)(Math.Sqrt(magnitude()));
	    }

	    public Point2Dim unitVec()
        {
		    float tmp = normVec();
		    if (tmp == 0)
			    return null;
		    return new Point2Dim(x/tmp,y/tmp);
	    }

	    public Point2Dim dotMulti(Point2Dim a)
        {
		    return new Point2Dim(x*a.x,y*a.y);
	    }

    }
}
