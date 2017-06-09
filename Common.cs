using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unigine;

namespace UnigineApp.Tool
{
    class Common
    {
        public delegate void Callback0();

        public static float Distance(dvec3 a, dvec3 b)
        {
            return (float)MathLib.length(a - b);
        }
        public static float Distance(vec3 a, vec3 b)
        {
            return (float)MathLib.length(a - b);
        }
        public static WorldIntersectionNormal intersection = new WorldIntersectionNormal();

        public static string DistanceToString(vec3 a, vec3 b)
        {
            double length = MathLib.length(a - b);
            return (length == 0) ? length.ToString() : length.ToString("f2");
        }

        /// <summary>
        /// 返回1是后面的参数高，返回0是前面的参数高。
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int CompareHigh(vec3 a, vec3 b)
        {
            if (a.z > b.z)
            {
                return 0;
            }
            return 1;
        }

        public static void LoadXML()
        {
            Xml province = new Xml();
            if (province.load("XML/load1.xml") == 1)
            {
                for (int pnum = 0; pnum < province.getNumChildren(); pnum++)
                {
                    Xml city = province.getChild(pnum);
                    Log.message(city.getData() + city.getNumChildren() + "\n");
                    for (int cnum = 0; cnum < city.getNumChildren(); cnum++)
                    {

                        Xml district = city.getChild(cnum);
                        Log.message(district.getData() + district.getNumChildren() + "\n");
                        for (int dnum = 0; dnum < district.getNumChildren(); dnum++)
                        {
                            Xml Modular = district.getChild(dnum);
                            NodeReference ModularNodeRef = new NodeReference("node/node_Modular.node");

                            //WorldManager.Get().AddList(ModularNodeRef);
                            //List<NodeReference> citylist = new List<NodeReference>();
                            //for (int mnum = 0; mnum < Modular.getNumChildren(); mnum++)
                            //{
                            //    Xml building = Modular.getChild(mnum);
                            //    if ("building".Equals(building.getName()))
                            //    {
                            //        NodeReference buildingNodeRef = new NodeReference("node/node_1.node");
                            //        buildingNodeRef.setParent(ModularNodeRef.getNode());
                            //        buildingNodeRef.setName(building.getChild(0).getData());
                            //        buildingNodeRef.setWorldPosition(building.getChild(1).getVec3Data());
                            //        citylist.Add(buildingNodeRef);
                            //    }
                            //    else if ("position".Equals(building.getName()))
                            //    {
                            //        building.getIntData();
                            //    }

                            //}
                            //WorldManager.Get().AddDictionary(ModularNodeRef, citylist);
                        }

                    }

                }
            }
        }

        public static NodeReference AddBuild(string path)
        {
            return new NodeReference(path);
        }

        public static float Limit(float value, float max, float min)
        {
            if (value > max)
            {
                value = max;
            }
            else if (value < min)
            {
                value = min;
            }
            return value;
        }

        public static vec3 Normalize(vec3 s)
        {
            s.normalize();
            return s;
        }
        public static dvec3 Normalize(dvec3 s)
        {
            s.normalize();
            return s;
        }

        public static WorldIntersectionNormal GetRay()
        {
            dvec3 p0;
            dvec3 p1;
            Game.get().getPlayer().getDirectionFromScreen(out p0, out  p1);
            World.get().getIntersection(p0, p1, 1, intersection);
            return intersection;
        }

        public static WorldIntersectionNormal GetRay(dvec3 point, dvec3 targetPint)
        {
            if (World.get().getIntersection(point, targetPint, 1, intersection) != null)
            {
                return intersection;
            }
            return null;

        }

        public static int Rand(int minValue, int maxValue)
        {
            Random ran = new Random();
            return ran.Next(minValue, maxValue);
        }

        public static int LeftFind(float[] f, float v)
        {
            for (int i = 0; i < f.Length; i++)
            {
                if (f[i] == v)
                {
                    return i;
                }
            }
            return -1;
        }

        public static quat LookAt(vec3 startposition, vec3 endposition)
        {
            return MathLib.normalize(new quat(MathLib.setTo(vec3.ZERO, endposition - startposition, vec3.UP)));
        }
        public static quat LookAt(dvec3 startposition, dvec3 endposition)
        {
            return MathLib.normalize(new quat(MathLib.setTo(dvec3.ZERO, endposition - startposition, vec3.UP)));
        }

        public static float area(dvec3 x1, dvec3 x2, dvec3 x3)
        {

            return (float)MathLib.abs(x1.x * (x2.y - x3.y) + x2.x * (x3.y - x1.y) + x3.x * (x1.y - x2.y)) * 0.5f;
        }

        public static float area(vec3 x1, vec3 x2, vec3 x3)
        {

            return MathLib.abs(x1.x * (x2.y - x3.y) + x2.x * (x3.y - x1.y) + x3.x * (x1.y - x2.y)) * 0.5f;
        }

        public static vec3 centre(List<vec3> listPoint)
        {
            vec3 contrepoint = vec3.ZERO;
            foreach (var item in listPoint)
            {
                contrepoint.x += item.x;
                contrepoint.y += item.y;
                contrepoint.z += item.z;
            }
            contrepoint /= listPoint.Count();
            return contrepoint;
        }

        public static float floatAreaCalc(List<vec3> dvlist)
        {
            float fArea = 0;
            for (int iCycle = 0; iCycle < dvlist.Count; iCycle++)
            {
                fArea += (dvlist[iCycle].x * dvlist[(iCycle + 1) % dvlist.Count].y - dvlist[(iCycle + 1) % dvlist.Count].x * dvlist[iCycle].y);
            }
            return MathLib.abs(0.5f * fArea);
        }
    }
}
