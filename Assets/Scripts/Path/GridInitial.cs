using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInitial : MonoBehaviour {
    public bool displayGridGizmos;
    public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	Node[,] grid;
    Pathfinding pathfinding;
    bool allowDiagonal;

	float nodeDiameter;
	int gridSizeX, gridSizeY;

	void Awake() {
        pathfinding = GetComponent<Pathfinding>();

		nodeDiameter = nodeRadius*2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
		CreateGrid();
	}

    void Update() {
        allowDiagonal = pathfinding.allowDiagonal;
		CreateGrid();
    }

    public int MaxSize {
        get {
            return gridSizeX * gridSizeY;
        }
    }

	void CreateGrid() {
		Vector3 range = new Vector3(0.25f, 0.25f, 0.25f);

		grid = new Node[gridSizeX,gridSizeY];
		Vector3 worldBottomLeft = (transform.position
								   - Vector3.right
								   * gridWorldSize.x
								   / 2
								   - Vector3.forward
								   * gridWorldSize.y
								   / 2);

		for (int x = 0; x < gridSizeX; x ++) {
			for (int y = 0; y < gridSizeY; y ++) {
                bool walkable;
				Vector3 worldPoint = (worldBottomLeft
                          			  + Vector3.right
                          			  * (x * nodeDiameter + nodeRadius)
                          			  + Vector3.forward
                          			  * (y * nodeDiameter + nodeRadius));
                if (allowDiagonal) {
                    walkable = !(Physics.CheckBox(worldPoint,
												  range,
												  new Quaternion(0,0,0,0),
												  unwalkableMask));
                } else {
				    walkable = !(Physics.CheckBox(worldPoint,
												  range,
												  new Quaternion(0,0,0,0),
												  unwalkableMask));
                }
				grid[x,y] = new Node(walkable, worldPoint, x, y);
			}
		}
	}

	public List<Node> GetNeighbours(Node node) {
		List<Node> neighbours = new List<Node>();
		bool allowDiagonal = true;

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				int checkX = node.gridX + x;
				int checkY = node.gridY + y;
				if (checkX >= 0
        			&& checkX < gridSizeX
        			&& checkY >= 0
        			&& checkY < gridSizeY) {
					Node checkNode = grid[checkX,checkY];
					allowDiagonal = checkNode.walkable;
					if (!allowDiagonal) {
						break;
					}
				}
			}
			if (!allowDiagonal) {
				break;
			}
		}
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
                if (allowDiagonal) {
                    if (x == 0 && y == 0) {
                        continue;
                    }
                } else {
                    if ((x == 0 && y == 0)
                        || (x == -1 && y ==  1)
                        || (x == -1 && y == -1)
                        || (x ==  1 && y ==  1)
                        || (x ==  1 && y == -1)) {
                        continue;
                    }
                }

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if (checkX >= 0
        			&& checkX < gridSizeX
        			&& checkY >= 0
        			&& checkY < gridSizeY) {
					Node addNode = grid[checkX,checkY];
					neighbours.Add(addNode);
				}
			}
		}
		return neighbours;
	}
	

	public Node NodeFromWorldPoint(Vector3 worldPosition) {
		float percentX = ((worldPosition.x + gridWorldSize.x / 2)
						  / gridWorldSize.x);
		float percentY = ((worldPosition.z + gridWorldSize.y / 2)
						  / gridWorldSize.y);
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
		return grid[x,y];
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position,
					        new Vector3(gridWorldSize.x,1,gridWorldSize.y));
        if (grid != null && displayGridGizmos) {
            foreach (Node n in grid) {
                Gizmos.color = (n.walkable)?Color.white:Color.red;
                Gizmos.DrawCube(n.worldPosition,
								Vector3.one * (nodeDiameter-.1f));
            }
        }
	}
}
