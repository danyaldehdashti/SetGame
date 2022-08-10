using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCards : MonoBehaviour
{
    [Header("Dependency")]
    
    [SerializeField] private List<Card> allCards;


    #region PickUpVariables

    private bool _hasCommonColor = false;
    private bool _hasCommonNumber = false;
    private bool _hasCommonModel = false;
    private bool _hasCommonGeometricShapes = false;

    private bool _noCommonColor = false;
    private bool _noCommonNumber = false;
    private bool _noCommonModel = false;
    private bool _noGeometricShapesCommon = false;
    
    #endregion
    
    
    private void GetTheSelectedCardInformation(List<int> cardsSelectedByThePlayer)
    {
        _hasCommonColor = false;
        _hasCommonModel = false;
        _hasCommonNumber = false;
        _hasCommonGeometricShapes = false;
        
        _noCommonColor = false;
        _noCommonModel = false;
        _noCommonNumber = false;
        _noGeometricShapesCommon = false;
        
        
        int colorRed = 0, colorBlue = 0, colorYellow = 0;
        int numberOne = 0, numberTwo = 0, numberThree = 0;
        int emptyModel = 0, fullModel = 0, stripesModel = 0;
        int geometricShapesSquare = 0,geometricShapesCircle = 0,geometricShapesTriangle = 0;
        
        
        for (int i = 0; i < 3; i++)
        {
            //Color
            switch (allCards[cardsSelectedByThePlayer[i]].color)
            {
                case ColorEnum.Blue:
                    colorBlue++;
                    break;
                
                case ColorEnum.Red:
                    colorRed++;
                    break;
                
                case ColorEnum.Yellow:
                    colorYellow++;
                    break;
            }
            //Number
            switch (allCards[cardsSelectedByThePlayer[i]].number)
            {
                case NumberEnum.One:
                    numberOne++;
                    break;
                
                case NumberEnum.Two:
                    numberTwo++;
                    break;
                
                case NumberEnum.Three:
                    numberThree++;
                    break;
            }
            //Model
            switch (allCards[cardsSelectedByThePlayer[i]].model)
            {
                case ModelEnum.Empty:
                    emptyModel++;
                    break;
                
                case ModelEnum.Full:
                    fullModel++;
                    break;
                
                case ModelEnum.Stripes:
                    stripesModel++;
                    break;
            }
            //GeometricShapes
            switch (allCards[cardsSelectedByThePlayer[i]].geometricShapes)
            {
                case GeometricShapesEnum.Circle:
                    geometricShapesCircle++;
                    break;
                
                case GeometricShapesEnum.Square:
                    geometricShapesSquare++;
                    break;
                
                case GeometricShapesEnum.Triangle:
                    geometricShapesTriangle++;
                    break;
            }
        }
        
        
        #region SetCommonVariables

        //Common Variables
        if (colorBlue == 3 || colorRed == 3|| colorYellow == 3)
        {
            _hasCommonColor = true;
        }
        if (numberOne == 3 || numberTwo == 3 || numberThree == 3)
        {
            _hasCommonNumber = true;
        }
        if (emptyModel == 3 || fullModel == 3 || stripesModel == 3)
        {
            _hasCommonModel = true;
        }
        if (geometricShapesCircle == 3 || geometricShapesSquare == 3 || geometricShapesTriangle == 3)
        {
            _hasCommonGeometricShapes = true;
        }
       
        //NoCommon Variables
        if (colorBlue == 1 && colorRed == 1 && colorYellow == 1)
        {
            _noCommonColor = true;
        }
        if (numberOne == 1 && numberTwo == 1 && numberThree == 1)
        {
            _noCommonNumber = true;
        }
        if (emptyModel == 1 && fullModel == 1 && stripesModel == 1)
        {
            _noCommonModel = true;
        }
        if (geometricShapesCircle == 1 && geometricShapesSquare == 1 && geometricShapesTriangle == 1)
        {
            _noGeometricShapesCommon = true;
        }

        #endregion
    }
    
    
    public bool PickUpConditions( List<int> cardsSelectedByThePlayer)
    {
        GetTheSelectedCardInformation(cardsSelectedByThePlayer);

        bool isPickUpSuccessful = false;

        #region Card Withdrawal Conditions

        int countOfCommons = 0;
        int countOfDifferences = 0;
        
        if (_hasCommonColor)
        {
            countOfCommons++;
        }
        if(_hasCommonNumber)
        {
            countOfCommons++;
        }
        if (_hasCommonModel)
        {
            countOfCommons++;
        }
        if (_hasCommonGeometricShapes)
        {
            countOfCommons++;
        }
        if (_noCommonColor)
        {
            countOfDifferences++;
        }
        if (_noCommonModel)
        {
            countOfDifferences++;
        }
        if (_noCommonNumber)
        {
            countOfDifferences++;

        }
        if (_noGeometricShapesCommon)
        {
            countOfDifferences++;
        }

        
        #endregion
        
        if (countOfCommons == 4 || countOfCommons == 3 || countOfCommons == 2 && countOfDifferences == 2 ||
            countOfCommons == 1 && countOfDifferences == 3 || countOfDifferences == 4)
        {
            isPickUpSuccessful = true;
        }

        return isPickUpSuccessful;
    }
}
