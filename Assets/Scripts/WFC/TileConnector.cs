using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WFC
{
    public static class TileConnector
    {
        public static void SetNeighbors(Tileset tileset)
        {
            Dictionary<Module, Color[]> colourIndex = new Dictionary<Module, Color[]>();

            for (int i = 0; i < tileset.modules.Length; i++)
            {
                Module curModule = tileset.modules[i];

                if (colourIndex.ContainsKey(curModule) == false)
                {
                    colourIndex.Add(curModule, new Color[16]);
                    Sprite image = curModule.image;
                    var croppedTexture = new Texture2D((int)image.rect.width, (int)image.rect.height);
                    var pixels = image.texture.GetPixels((int)image.textureRect.x,
                                                            (int)image.textureRect.y,
                                                            (int)image.textureRect.width,
                                                            (int)image.textureRect.height);
                    croppedTexture.SetPixels(pixels);
                    croppedTexture.Apply();

                    //North
                    colourIndex[curModule][0] = croppedTexture.GetPixel(0, 7);
                    colourIndex[curModule][1] = croppedTexture.GetPixel(3, 7);
                    colourIndex[curModule][2] = croppedTexture.GetPixel(4, 7);
                    colourIndex[curModule][3] = croppedTexture.GetPixel(7, 7);

                    //East
                    colourIndex[curModule][4] = croppedTexture.GetPixel(7, 7);
                    colourIndex[curModule][5] = croppedTexture.GetPixel(7, 4);
                    colourIndex[curModule][6] = croppedTexture.GetPixel(7, 3);
                    colourIndex[curModule][7] = croppedTexture.GetPixel(7, 0);

                    //South
                    colourIndex[curModule][8] = croppedTexture.GetPixel(0, 0);
                    colourIndex[curModule][9] = croppedTexture.GetPixel(3, 0);
                    colourIndex[curModule][10] = croppedTexture.GetPixel(4, 0);
                    colourIndex[curModule][11] = croppedTexture.GetPixel(7, 0);

                    //West
                    colourIndex[curModule][12] = croppedTexture.GetPixel(0, 7);
                    colourIndex[curModule][13] = croppedTexture.GetPixel(0, 4);
                    colourIndex[curModule][14] = croppedTexture.GetPixel(0, 3);
                    colourIndex[curModule][15] = croppedTexture.GetPixel(0, 0);
                }
            }

            List<Module> northN = new List<Module>();
            List<Module> eastN = new List<Module>();
            List<Module> southN = new List<Module>();
            List<Module> westN = new List<Module>();

            for (int cnt = 0; cnt < tileset.modules.Length; cnt++)
            {
                Module curModule = tileset.modules[cnt];
                northN.Clear();
                eastN.Clear();
                southN.Clear();
                westN.Clear();

                for (int vnt = 0; vnt < tileset.modules.Length; vnt++)
                {
                    Module temp = tileset.modules[vnt];


                    if (DoesMatch(colourIndex, curModule, temp, 0, 8))
                    {
                        northN.Add(temp);
                    }

                    if (DoesMatch(colourIndex, curModule, temp, 4, 12))
                    {
                        eastN.Add(temp);
                    }

                    if (DoesMatch(colourIndex, curModule, temp, 8, 0))
                    {
                        southN.Add(temp);
                    }

                    if (DoesMatch(colourIndex, curModule, temp, 12, 4))
                    {
                        westN.Add(temp);
                    }

                }

                curModule.north = northN.ToArray();
                curModule.east = eastN.ToArray();
                curModule.south = southN.ToArray();
                curModule.west = westN.ToArray();
            }
        }

        private static bool DoesMatch(Dictionary<Module, Color[]> colourIndex, Module cur, Module temp, int xStart, int yStart)
        {
            for (int i = 0; i <= 3; i++)
            {
                if (colourIndex[cur][xStart + i] != colourIndex[temp][yStart + i])
                {
                    //Debug.Log("False");
                    return false;
                }
            }


            //Debug.Log("True");
            return true;
        }
    }
}
