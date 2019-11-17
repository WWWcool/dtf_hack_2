using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.Tilemaps
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Back Tile", menuName = "Tiles/Back Tile")]
    public class BackTile : TileBase
    {
        [SerializeField] public Sprite[] m_Sprites;
        public static int startUpCalls = 0;

        public override void RefreshTile(Vector3Int location, ITilemap tileMap)
        {
            for (int yd = -1; yd <= 1; yd++)
                for (int xd = -1; xd <= 1; xd++)
                {
                    Vector3Int position = new Vector3Int(location.x + xd, location.y + yd, location.z);
                    if (TileValue(tileMap, position))
                        tileMap.RefreshTile(position);
                }
        }

        public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
        {
            // Debug.Log("[BackTile][GetTileData]");
            UpdateTile(location, tilemap, ref tileData);
        }

        public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject go)
        {
            startUpCalls++;
            return true;
        }

        private void UpdateTile(Vector3Int location, ITilemap tileMap, ref TileData tileData)
        {
            tileData.transform = Matrix4x4.identity;
            tileData.color = Color.white;

            int mask = TileValue(tileMap, location + new Vector3Int(0, 1, 0)) ? 1 : 0;
            mask += TileValue(tileMap, location + new Vector3Int(1, 1, 0)) ? 2 : 0;
            mask += TileValue(tileMap, location + new Vector3Int(1, 0, 0)) ? 4 : 0;
            mask += TileValue(tileMap, location + new Vector3Int(1, -1, 0)) ? 8 : 0;
            mask += TileValue(tileMap, location + new Vector3Int(0, -1, 0)) ? 16 : 0;
            mask += TileValue(tileMap, location + new Vector3Int(-1, -1, 0)) ? 32 : 0;
            mask += TileValue(tileMap, location + new Vector3Int(-1, 0, 0)) ? 64 : 0;
            mask += TileValue(tileMap, location + new Vector3Int(-1, 1, 0)) ? 128 : 0;
            // Debug.Log($"[BackTile][GetTileData] loc {location.ToString()} mask {mask}");

            int index = GetIndex((byte)mask);
            if (index >= 0 && index < m_Sprites.Length && TileValue(tileMap, location))
            {
                tileData.sprite = m_Sprites[index];
                tileData.transform = Matrix4x4.identity;
                tileData.color = Color.white;
                tileData.flags = TileFlags.LockTransform | TileFlags.LockColor;
                tileData.colliderType = Tile.ColliderType.Sprite;
            }
        }

        private bool TileValue(ITilemap tileMap, Vector3Int position)
        {
            TileBase tile = tileMap.GetTile(position);
            return (tile != null && tile == this);
        }

        private int GetIndex(byte mask)
        {
            switch (mask)
            {
                case 71:
                case 197:
                case 199: return 0;
                case 196: return 1;
                case 70: return 2;
                case 229:
                case 231: return 3;
                case 79:
                case 207: return 4;
                case 100: return 5;
                case 17:
                case 19:
                case 21:
                case 23:
                case 25:
                case 27:
                case 29:
                case 31:
                case 49:
                case 51:
                case 53:
                case 57:
                case 59:
                case 63:
                case 81:
                case 83:
                case 89:
                case 93:
                case 95:
                case 113:
                case 117:
                case 125:
                case 127:
                case 145:
                case 147:
                case 149:
                case 151:
                case 153:
                case 159:
                case 185:
                case 209:
                case 211:
                case 215:
                case 223:
                case 241:
                case 245:
                case 243:
                case 247:
                case 249:
                case 253:
                case 255: return 6;
                case 244:
                case 252: return 7;
                case 94:
                case 126: return 8;
                case 109:
                case 239: return 9;
                case 238:
                case 108: return 10;
                case 198: return 11;
                case 5:
                case 7:
                case 135: return 12;
                case 65:
                case 193:
                case 195: return 13;
                case 20:
                case 28:
                case 190:
                case 60: return 14;
                case 80:
                case 112:
                case 250:
                case 120: return 15;
                case 84:
                case 92:
                case 116:
                case 254:
                case 124: return 16;
                case 76: return 17;
                case 0:
                case 2:
                case 8:
                case 32:
                case 136:
                case 34:
                case 128: return 18;
                case 1:
                case 3:
                case 33:
                case 129:
                case 131: return 19;
                case 68: return 20;
                case 142:
                case 174:
                case 46:
                case 14: return 21;
                case 6: return 22;
                case 4: return 23;
                case 78: return 24;
                case 12: return 25;
                case 44:
                case 234:
                case 232:
                case 226:
                case 224: return 26;
                case 194:
                case 192: return 27;
                case 64: return 28;
                case 228: return 29;
                case 104:
                case 96: return 30;
                case 16:
                case 24:
                case 48:
                case 144:
                case 56: return 31;
                case 97:
                case 225:
                case 227: return 32;
                case 13:
                case 15:
                case 143: return 33;
                case 22:
                case 150:
                case 30:
                case 62: return 34;
                case 208:
                case 210:
                case 240:
                case 248: return 35;
                
            }
            Debug.Log($"[BackTile][GetIndex] error can`t find index for mask {mask}");
            return -1;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(BackTile))]
    public class BackTileEditor : Editor
    {
        private BackTile tile { get { return (target as BackTile); } }

        public void OnEnable()
        {
            if (tile.m_Sprites == null || tile.m_Sprites.Length != 36)
            {
                tile.m_Sprites = new Sprite[36];
                EditorUtility.SetDirty(tile);
            }
        }


        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Place sprites shown based on the contents of the sprite.");
            EditorGUILayout.Space();

            float oldLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 210;

            EditorGUI.BeginChangeCheck();
            tile.m_Sprites[0] = (Sprite)EditorGUILayout.ObjectField("tile_border_center", tile.m_Sprites[0], typeof(Sprite), false, null);
            tile.m_Sprites[1] = (Sprite)EditorGUILayout.ObjectField("tile_border_corner_left", tile.m_Sprites[1], typeof(Sprite), false, null);
            tile.m_Sprites[2] = (Sprite)EditorGUILayout.ObjectField("tile_border_corner_right", tile.m_Sprites[2], typeof(Sprite), false, null);
            tile.m_Sprites[3] = (Sprite)EditorGUILayout.ObjectField("tile_border_inner_top_left", tile.m_Sprites[3], typeof(Sprite), false, null);
            tile.m_Sprites[4] = (Sprite)EditorGUILayout.ObjectField("tile_border_inner_top_right", tile.m_Sprites[4], typeof(Sprite), false, null);
            tile.m_Sprites[5] = (Sprite)EditorGUILayout.ObjectField("tile_border_left_top_corner", tile.m_Sprites[5], typeof(Sprite), false, null);
            tile.m_Sprites[6] = (Sprite)EditorGUILayout.ObjectField("tile_border_main", tile.m_Sprites[6], typeof(Sprite), false, null);
            tile.m_Sprites[7] = (Sprite)EditorGUILayout.ObjectField("tile_border_main_top_left", tile.m_Sprites[7], typeof(Sprite), false, null);
            tile.m_Sprites[8] = (Sprite)EditorGUILayout.ObjectField("tile_border_main_top_right", tile.m_Sprites[8], typeof(Sprite), false, null);
            tile.m_Sprites[9] = (Sprite)EditorGUILayout.ObjectField("tile_border_opened_on_bottom", tile.m_Sprites[9], typeof(Sprite), false, null);
            tile.m_Sprites[10] = (Sprite)EditorGUILayout.ObjectField("tile_border_opened_on_bottom_and_top", tile.m_Sprites[10], typeof(Sprite), false, null);
            tile.m_Sprites[11] = (Sprite)EditorGUILayout.ObjectField("tile_border_opened_on_top", tile.m_Sprites[11], typeof(Sprite), false, null);
            tile.m_Sprites[12] = (Sprite)EditorGUILayout.ObjectField("tile_border_outer_bottom_left", tile.m_Sprites[12], typeof(Sprite), false, null);
            tile.m_Sprites[13] = (Sprite)EditorGUILayout.ObjectField("tile_border_outer_bottom_right", tile.m_Sprites[13], typeof(Sprite), false, null);
            tile.m_Sprites[14] = (Sprite)EditorGUILayout.ObjectField("tile_border_outer_left_up", tile.m_Sprites[14], typeof(Sprite), false, null);
            tile.m_Sprites[15] = (Sprite)EditorGUILayout.ObjectField("tile_border_outer_right_up", tile.m_Sprites[15], typeof(Sprite), false, null);
            tile.m_Sprites[16] = (Sprite)EditorGUILayout.ObjectField("tile_border_outer_top_center", tile.m_Sprites[16], typeof(Sprite), false, null);
            tile.m_Sprites[17] = (Sprite)EditorGUILayout.ObjectField("tile_border_right_top_corner", tile.m_Sprites[17], typeof(Sprite), false, null);
            tile.m_Sprites[18] = (Sprite)EditorGUILayout.ObjectField("tile_border_single", tile.m_Sprites[18], typeof(Sprite), false, null);
            tile.m_Sprites[19] = (Sprite)EditorGUILayout.ObjectField("tile_border_single_bottom", tile.m_Sprites[19], typeof(Sprite), false, null);
            tile.m_Sprites[20] = (Sprite)EditorGUILayout.ObjectField("tile_border_single_line", tile.m_Sprites[20], typeof(Sprite), false, null);
            tile.m_Sprites[21] = (Sprite)EditorGUILayout.ObjectField("tile_border_single_side_left", tile.m_Sprites[21], typeof(Sprite), false, null);
            tile.m_Sprites[22] = (Sprite)EditorGUILayout.ObjectField("tile_border_single_side_left_bottom", tile.m_Sprites[22], typeof(Sprite), false, null);
            tile.m_Sprites[23] = (Sprite)EditorGUILayout.ObjectField("tile_border_single_side_left_end", tile.m_Sprites[23], typeof(Sprite), false, null);
            tile.m_Sprites[24] = (Sprite)EditorGUILayout.ObjectField("tile_border_single_side_left_opened", tile.m_Sprites[24], typeof(Sprite), false, null);
            tile.m_Sprites[25] = (Sprite)EditorGUILayout.ObjectField("tile_border_single_side_left_top", tile.m_Sprites[25], typeof(Sprite), false, null);
            tile.m_Sprites[26] = (Sprite)EditorGUILayout.ObjectField("tile_border_single_side_right", tile.m_Sprites[26], typeof(Sprite), false, null);
            tile.m_Sprites[27] = (Sprite)EditorGUILayout.ObjectField("tile_border_single_side_right_bottom", tile.m_Sprites[27], typeof(Sprite), false, null);
            tile.m_Sprites[28] = (Sprite)EditorGUILayout.ObjectField("tile_border_single_side_right_end", tile.m_Sprites[28], typeof(Sprite), false, null);
            tile.m_Sprites[29] = (Sprite)EditorGUILayout.ObjectField("tile_border_single_side_right_opened", tile.m_Sprites[29], typeof(Sprite), false, null);
            tile.m_Sprites[30] = (Sprite)EditorGUILayout.ObjectField("tile_border_single_side_right_top", tile.m_Sprites[30], typeof(Sprite), false, null);
            tile.m_Sprites[31] = (Sprite)EditorGUILayout.ObjectField("tile_border_single_top", tile.m_Sprites[31], typeof(Sprite), false, null);
            tile.m_Sprites[32] = (Sprite)EditorGUILayout.ObjectField("tile_border_step_bottom_left", tile.m_Sprites[32], typeof(Sprite), false, null);
            tile.m_Sprites[33] = (Sprite)EditorGUILayout.ObjectField("tile_border_step_bottom_right.png", tile.m_Sprites[33], typeof(Sprite), false, null);
            tile.m_Sprites[34] = (Sprite)EditorGUILayout.ObjectField("tile_border_step_top_left", tile.m_Sprites[34], typeof(Sprite), false, null);
            tile.m_Sprites[35] = (Sprite)EditorGUILayout.ObjectField("tile_border_step_top_right", tile.m_Sprites[35], typeof(Sprite), false, null);

            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(tile);

            EditorGUIUtility.labelWidth = oldLabelWidth;
        }
    }
#endif
}
