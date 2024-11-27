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
    public Vector3 cellScale;
    public bool showGrid;
    void OnEnable()
    {
        InitializeGrid();
        if(showGrid)DrawGrid();
        //VisualizeCellCenters();
        cellScale = new Vector3(cellSize, cellSize, 1);

    }

    void InitializeGrid()
    {
        // Get bottom-left and top-right world positions based on the camera
        bottomLeft = cam.ScreenToWorldPoint(new Vector2(0, 0));
        topRight = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        // Calculate usable grid dimensions, considering padding
        float gridWidth = topRight.x - bottomLeft.x - leftPadding;
        float gridHeight = topRight.y - bottomLeft.y - topPadding - bottomPadding;

        // Determine cell size (ensures square cells)
        cellSize = Mathf.Min(gridWidth / GameData.cols, gridHeight / GameData.rows);

        // Calculate grid starting points
        xStart = bottomLeft.x + leftPadding;
        yStart = topRight.y - topPadding;

        // Debugging for validation
        Debug.Log($"Grid initialized: CellSize={cellSize}, XStart={xStart}, YStart={yStart}");
    }


    public Vector2 GetCellCenter(int row, int column)
    {
        // Validate row and column bounds
        if (row < 0 || row >= GameData.rows || column < 0 || column >= GameData.cols)
        {
            Debug.LogError($"Invalid row or column: row={row}, column={column}");
            return Vector2.zero;
        }

        // Calculate center positions
        float xCenter = xStart + (column * cellSize) + (cellSize / 2);
        float yCenter = yStart - (row * cellSize) - (cellSize / 2);

        // Debugging for validation
        Debug.Log($"Cell Center for row={row}, column={column}: ({xCenter}, {yCenter})");
        return new Vector2(xCenter, yCenter);
    }

    void VisualizeCellCenters()
    {
        for (int row = 0; row < GameData.rows; row++)
        {
            for (int col = 0; col < GameData.cols; col++)
            {
                Vector2 center = GetCellCenter(row, col);
                GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                marker.transform.position = center;
                marker.transform.localScale = Vector3.one * cellSize * 0.5f;
            }
        }
    }


    void DrawGrid()
    {
        for (int i = 0; i <= GameData.rows; i++)
        {
            float yPos = yStart - (i * cellSize);
            DrawLine(new Vector2(xStart, yPos), new Vector2(xStart + (cellSize * GameData.cols), yPos));
        }
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
