using UnityEngine;
using System.Linq;

public class Generation : MonoBehaviour
{
    [SerializeField] private GeneratedObject[] genObjects;

    [Header("Set Field")]
    [SerializeField] private Vector2Int fieldSize;
    private bool[,] field;

    private void Start()
    {
        field = new bool[fieldSize.x, fieldSize.y];
        GenerateObjects();
    }

    private void GenerateObjects()
    {
        genObjects = genObjects.OrderBy(item => item.prefab.transform.localScale.x == 1).ToArray();

        foreach (GeneratedObject item in genObjects)
        {
            for (int i = 0; i < item.count; i++)
            {
                int size = (int)item.prefab.transform.localScale.x;

                int x = 0;
                int y = 0;

                int attempt = 0;
                while(attempt < 1000)
                {
                    x = Random.Range(0, fieldSize.x);
                    y = Random.Range(0, fieldSize.y);
                    if (field[x, y])
                    {
                        attempt++;
                        continue;
                    }
                    if (size != 1)
                    {
                        try
                        {
                            if (field[x + 1, y] || field[x, y + 1] || field[x + 1, y + 1])
                            {
                                attempt++;
                                continue;
                            }
                        }
                        catch
                        {
                            attempt++;
                            continue;
                        }
                    }

                    Vector2 startPos = new Vector2(-fieldSize.x / 2, fieldSize.y / 2);
                    Vector2 spawnPos = startPos + new Vector2(x, -y);
                    GameObject p = Instantiate(item.prefab, spawnPos, Quaternion.identity);
                    p.transform.SetParent(gameObject.transform);

                    field[x, y] = true;

                    if (size != 1)
                    {
                        field[x + 1, y] = true;
                        field[x, y + 1] = true;
                        field[x + 1, y + 1] = true;
                    }

                    break;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector2 startPos = new Vector2(-fieldSize.x / 2, fieldSize.y / 2);

        for (int i = 0; i <= fieldSize.x; i++)
        {
            Vector2 lineStart = startPos + new Vector2(i, 0);
            Vector2 lineEnd = lineStart + new Vector2(0, -fieldSize.y);
            Gizmos.DrawLine(lineStart, lineEnd);
        }

        for (int j = 0; j <= fieldSize.y; j++)
        {
            Vector2 lineStart = startPos + new Vector2(0, -j);
            Vector2 lineEnd = lineStart + new Vector2(fieldSize.x, 0);
            Gizmos.DrawLine(lineStart, lineEnd);
        }
    }
}