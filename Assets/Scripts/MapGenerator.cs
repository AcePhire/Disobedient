using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class GridData
{
    //2 dimensional array
    private ushort[,] grid;
    private int numberOfTiles, length, width;

    //to save the occupied to occupy the sides
    private List<ushort[]> occupiedList = new List<ushort[]>();

    //create the grid and initiate it
    public GridData(int numberOfTiles, int length, int width){
        this.numberOfTiles = numberOfTiles;
        this.length = length;
        this.width = width;

        grid = new ushort[length, width];

        for (ushort i = 0; i < length; i++){
            for (ushort j = 0; j < width; j++){
                //(y,x)
                grid[i, j] = 0;
            }
        }

        randomGenerate();
        generateEdges();
    }

    public ushort[,] getGridData(){
        return grid;
    }

    private void generateEdges(){
        generateHorizontalEdges();

        generateVerticalEdges();

        generateFinalEdges();
    }

    private void generateHorizontalEdges(){
        //create a list of the rows, each containing its y-axis, so we can remove that row, once finished with
        List<int> rows = new List<int>();
        //we create an array of the value of edges of each row, and we set it to 0
        int[] rowsEdges = new int[length];
        for (int i = 0; i < length; i++){
             rows.Add(i);
             rowsEdges[i] = 0;
        }
        
        //we create a max number of columns
        ushort max = (ushort)(width - 1);

        //while all the rows are there(edges still not filled)
        while (max > 0){
            //go through each row
            for (int i = 0; i < rows.Count; i++){

                //get the first and the last cells, first's y = (the total amount of columns) - (the cureent max number of columns)
                ushort[] tileFirst = new ushort[] {(ushort)rows[i], (ushort)((width - 1) - max)};
                ushort[] tileLast = new ushort[] {(ushort)rows[i], max};

                //if the first cell is occupied(might be the edge) and the value of the edge is not -1(which means the left edge hasnt been placed) and its not 1(which means both edges arent filled)
                if (occupied(tileFirst) && rowsEdges[rows[i]] != -1 && rowsEdges[rows[i]] != 1){
                    //make it 2 on the gridData and -1 to the value of the edge to make it -1 or 1 if the last cell was filled
                    grid[tileFirst[0], tileFirst[1]] = 2;
                    rowsEdges[rows[i]] -= 1;
                }

                //if the last cell is occupied(might be the edge) and the value of the edge is not 2(which means the right edge hasnt been placed) and its not 1(which means both edges arent filled)
                if (occupied(tileLast) && rowsEdges[rows[i]] != 2 && rowsEdges[rows[i]] != 1){
                    //make it 2 on the gridData and +2 to the value of the edge to make it 2 or 1 if the last cell was filled
                    grid[tileFirst[0], tileLast[1]] = 2;
                    rowsEdges[rows[i]] += 2;
                }
            }

            //go through each row and remove the ones with edge value of 1 which means both are filled
            for (int i = 0; i < rows.Count; i++){
                if (rowsEdges[rows[i]] == 1){
                    rows.RemoveAt(i);
                }
            }

            //lower the max by one to go to the next column on both sides
            max--;
        }  
    }

    private void generateVerticalEdges(){
        List<int> columns = new List<int>();
        int[] columnsEdges = new int[width];
        for (int i = 0; i < width; i++){
             columns.Add(i);
             columnsEdges[i] = 0;
        }
        
        ushort max = (ushort)(length - 1);

        while (max > 0){
            for (int i = 0; i < columns.Count; i++){

                ushort[] tileFirst = new ushort[] { (ushort)((length - 1) - max), (ushort)columns[i]};
                ushort[] tileLast = new ushort[] {max, (ushort)columns[i]};

                if (occupied(tileFirst) && columnsEdges[columns[i]] != -1 && columnsEdges[columns[i]] != 1){
                    grid[tileFirst[0], tileFirst[1]] = 2;
                    columnsEdges[columns[i]] -= 1;
                }

                if (occupied(tileLast) && columnsEdges[columns[i]] != 2 && columnsEdges[columns[i]] != 1){
                    grid[tileLast[0], tileLast[1]] = 2;
                    columnsEdges[columns[i]] += 2;
                }
            }

            for (int i = 0; i < columns.Count; i++){
                if (columnsEdges[columns[i]] == 1){
                    columns.RemoveAt(i);
                }
            }

            max--;
        }
    }

    private void generateFinalEdges(){
        //make a list of all edges
        List<ushort[]> edgeList = new List<ushort[]>();

        for (int i = 0; i < length; i++){
            for (int j = 0; j < width; j++){
                if (grid[i, j] > 1) edgeList.Add(new ushort[]{(ushort)i, (ushort)j});
            }
        }
        
        //go through each edge and check its surroundings for empty cells, if it is, then make it an edge and add it to the edge list, remove the current edge after checking the surrounding
        while (edgeList.Count > 0){
            for (int i = 0; i < edgeList.Count; i++){
                ushort[] tempEdge = edgeList[i];
                if (tempEdge[1] < width-1 && !occupied(rightOf(tempEdge))){
                    makeEdge(rightOf(tempEdge));
                    edgeList.Add(rightOf(tempEdge));
                } 
                if (tempEdge[0] > 0 && !occupied(upOf(tempEdge))){
                    makeEdge(upOf(tempEdge));
                    edgeList.Add(upOf(tempEdge));
                } 
                if (tempEdge[1] > 0 && !occupied(leftOf(tempEdge))){
                    makeEdge(leftOf(tempEdge));
                    edgeList.Add(leftOf(tempEdge));
                } 
                if (tempEdge[0] < length-1 && !occupied(downOf(tempEdge))){
                    makeEdge(downOf(tempEdge));
                    edgeList.Add(downOf(tempEdge));
                } 
                edgeList.RemoveAt(i);
            }
        }
    }

    private void randomGenerate(){
        //start from the middle
        addTile(new ushort[]{(ushort)(length/2), (ushort)(width/2)});

        //while there is tiles to occupy
        while (numberOfTiles > 0){

            //create a temp list for the occupied
            List<ushort[]> tempList = occupiedList;

            //go through each occupied and occupy the surroundings randomly
            for(int i = 0; i < tempList.Count; i++){
                ushort[] tempTile = tempList[i];

                //create a list of the positions of the actions
                List<int> actsList = new List<int>(){0, 1, 2, 3};
                for (int j = 0; j < 4; j++){
                    //take a random position
                    int actNumber = Random.Range(0, actsList.Count);

                    //run the action in that random position
                    if (actsList[actNumber] == 0 && tempTile[1] < width-1 && !occupied(rightOf(tempTile)) && numberOfTiles > 0) addTile(rightOf(tempTile));
                    if (actsList[actNumber] == 1 && tempTile[0] > 0 && !occupied(upOf(tempTile)) && numberOfTiles > 0) addTile(upOf(tempTile));
                    if (actsList[actNumber] == 2 && tempTile[1] > 0 && !occupied(leftOf(tempTile)) && numberOfTiles > 0) addTile(leftOf(tempTile));
                    if (actsList[actNumber] == 3 && tempTile[0] < length-1 && !occupied(downOf(tempTile)) && numberOfTiles > 0) addTile(downOf(tempTile));

                    //remove the position so it doesnt repeat
                    actsList.RemoveAt(actNumber);
                }
                //remove the tile once finished all surroundings
                occupiedList.Remove(tempTile);
            }  
        }  
    }

    private void fixedGenerate(){
        //start from the middle
        addTile(new ushort[]{(ushort)(length/2), (ushort)(width/2)});

        //while there is tiles to occupy
        while (numberOfTiles > 0){

            //create a temp list for the occupied
            List<ushort[]> tempList = occupiedList;

            //go through each occupied and occupy the surroundings randomly
            for(int i = 0; i < tempList.Count; i++){
                ushort[] tempTile = tempList[i];

                if (tempTile[1] < width-1 && !occupied(rightOf(tempTile)) && numberOfTiles > 0) addTile(rightOf(tempTile));
                if (tempTile[0] > 0 && !occupied(upOf(tempTile)) && numberOfTiles > 0) addTile(upOf(tempTile));
                if (tempTile[1] > 0 && !occupied(leftOf(tempTile)) && numberOfTiles > 0) addTile(leftOf(tempTile));
                if (tempTile[0] < length-1 && !occupied(downOf(tempTile)) && numberOfTiles > 0) addTile(downOf(tempTile));
                occupiedList.Remove(tempTile);
            }  
        }  
    }

    private void addTile(ushort[] tile){
        //update the array
        grid[tile[0], tile[1]] = 1;
        //add to occupied list
        occupiedList.Add(tile);
        //lower the number of tiles left
        numberOfTiles--;
    }

    private void makeEdge(ushort[] tile){
        grid[tile[0], tile[1]] = 2;
    }
 
    private bool occupied(ushort[] tile){
        return grid[tile[0], tile[1]] != 0;
    }

    private ushort[] rightOf(ushort[] tile){
        return new ushort[]{tile[0], (ushort)(tile[1]+1)};
    }

    private ushort[] upOf(ushort[] tile){
        return new ushort[]{(ushort)(tile[0]-1), tile[1]};
    }

    private ushort[] leftOf(ushort[] tile){
        return new ushort[]{tile[0], (ushort)(tile[1]-1)};
    }

    private ushort[] downOf(ushort[] tile){
        return new ushort[]{(ushort)(tile[0]+1), tile[1]};
    }
}

public class MapGenerator : MonoBehaviour
{
    public Spawner spawner;

    public GameObject ground;

    public GameObject grid;

    public GameObject bush;

    public Tile[] grassTiles = new Tile[4];
    
    public int length, width;

    private int filledPercentage;
    private int sizeOfTile = 15;

    // Start is called before the first frame update
    void Awake()
    {
        //get length and width
        length = Manager.dimensions[0];
        width = Manager.dimensions[1];

        int area = length*width;

        //curve the filled percentage             1.60406f x ln(2.95886√x - 54.2259) + 5.43952
        filledPercentage = 95 - Mathf.RoundToInt((1.60406f * Mathf.Log(2.95886f * Mathf.Sqrt(area) - 54.2259f) + 5.43952f));

        int filled = (filledPercentage*area)/100;
        if (filled > area) filled = area;
        
        //create the map with data
        GridData gridData = new GridData(filled, length, width);
        //display map from data
        createTilemap(gridData.getGridData(), length, width);
        
        //set the size of each tile and center the map
        grid.GetComponent<Grid>().cellSize = new Vector3(sizeOfTile, sizeOfTile);
        grid.transform.position = new Vector2(-(width * sizeOfTile)/2, (length * sizeOfTile)/2);

        //change the size of the background to fit the map
        ground.transform.localScale = new Vector2((width*50/3)+100, (length*50/3)+100);
        ground.transform.localPosition = new Vector2((width * sizeOfTile)/2, -(length * sizeOfTile)/2);

        spawner.GetComponent<Spawner>().enabled = true;

        //set spawning area for enemies and items
        spawner.spawningArea = new Area(new Vector2[] {
            new Vector2(-(width*sizeOfTile/2) + 50, (length*sizeOfTile/2) - 50),
            new Vector2((width*sizeOfTile/2) - 50, (length*sizeOfTile/2) - 50),
            new Vector2(-(width*sizeOfTile/2) + 50, -(length*sizeOfTile/2) + 50),
            new Vector2((width*sizeOfTile/2) - 50, -(length*sizeOfTile/2) + 50)
        });
    }

    private void createTilemap(ushort[,] gridData, int length, int width){
        //tilemaps starts from the bottom left
        //create a tilemap object and set its parent to the grid
        Tilemap grassTilemap = new GameObject("grass").AddComponent<Tilemap>();
        grassTilemap.gameObject.AddComponent<TilemapRenderer>();
        grassTilemap.transform.SetParent(grid.transform);

        Tilemap bushTilemap = new GameObject("bush").AddComponent<Tilemap>();
        bushTilemap.gameObject.AddComponent<TilemapRenderer>();
        bushTilemap.gameObject.AddComponent<TilemapCollider2D>();
        bushTilemap.transform.SetParent(grid.transform);

        bush = bushTilemap.gameObject;

        //go through each position on the griddata and take the value, if 1 place a random tile of the 4
        for (int i = 0; i < length; i++){
            for (int j = 0; j < width; j++){
                if (gridData[i, j] == 2){
                    bushTilemap.SetTile(new Vector3Int(j, -i, 0), grassTiles[3]);
                }else if (gridData[i, j] == 1){   
                    grassTilemap.SetTile(new Vector3Int(j, -i, 0), grassTiles[Random.Range(0, 3)]);
                }
            }
        }
    }
}


