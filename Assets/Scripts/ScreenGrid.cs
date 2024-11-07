using UnityEngine;

public class ScreenGrid : MonoBehaviour
{
    public Color lineColor = Color.black; // Line color
    public float lineWidth; // Line width
    public float leftPadding; // Padding from the left (in world units)
    public float topPadding; // Padding from the top (in world units)
    public float bottomPadding; // Padding from the bottom (in world units)
    private float cellSize; // Size of each square cell
    private Vector2 bottomLeft; // Bottom left corner in world coordinates
    private Vector2 topRight; // Top right corner in world coordinates
    private float xStart; // Starting x position for the grid
    private float yStart; // Starting y position for the grid
    [SerializeField] Camera cam;

    void Start()
    {
        InitializeGrid();
        DrawGrid();
    }

    void InitializeGrid()
    {
        // Get screen boundaries in world units
        bottomLeft = cam.ScreenToWorldPoint(new Vector2(0, 0));
        topRight = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        // Calculate the usable grid width and height, accounting for paddings
        float gridWidth = topRight.x - bottomLeft.x - leftPadding;
        float gridHeight = topRight.y - bottomLeft.y - topPadding - bottomPadding;

        // Determine the cell size by taking the smaller dimension (ensuring square cells)
        cellSize = Mathf.Min(gridWidth / GameData.cols, gridHeight / GameData.rows);

        // Calculate x and y start positions based on padding and grid dimensions
        xStart = bottomLeft.x + leftPadding;
        yStart = topRight.y - topPadding;
    }

    public Vector2 GetCellCenter(int row, int column)
    {
        float xCenter = xStart + (column * cellSize) + (cellSize / 2);
        float yCenter = yStart - (row * cellSize) - (cellSize / 2);
        return new Vector2(xCenter, yCenter);
    }

    void DrawGrid()
    {
        // Draw horizontal lines
        for (int i = 0; i <= GameData.rows; i++)
        {
            float yPos = yStart - (i * cellSize);
            DrawLine(new Vector2(xStart, yPos), new Vector2(xStart + (cellSize * GameData.cols), yPos));
        }

        // Draw vertical lines
        for (int j = 0; j <= GameData.cols; j++)
        {
            float xPos = xStart + (j * cellSize);
            DrawLine(new Vector2(xPos, yStart), new Vector2(xPos, yStart - (cellSize * GameData.rows)));
        }
    }

    void DrawLine(Vector2 start, Vector2 end)
    {
        GameObject line = new GameObject("Line");
        LineRenderer lr = line.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        // Set LineRenderer properties
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = lineColor;
        lr.endColor = lineColor;
    }
}
