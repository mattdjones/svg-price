using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace priceSvg
{
    public class zapQuote
    {
        #region publicProperties

        //cut line lengths
        public double totalCutLength = 0;
        public int cutCount = 0;
        public double totalCutArea = 0;

        //engrave line lengths
        public double totalLightEngrave = 0;
        public int lightEngraveCount = 0;
        public double totalMedEngrave = 0;
        public int medEngraveCount = 0; 
        public double totalHeavyEngrave = 0;
        public int heavyEngraveCount = 0;
        public double totalLengthEngrave = 0;

        //raster fill area
        public double totalLightFill = 0;
        public int lightFillCount = 0;
        public double totalMedFill = 0;
        public int medFillCount = 0;
        public double totalHeavyFill = 0;
        public int heavyFillCount = 0;
        public double totalAreaFill = 0;
        public double rasterArea = 0;
        public double tempRasterHeight = 0;

        //pricing info
        public double cutTime = 0;
        public double engraveTime = 0;
        public double fillTime = 0;
        public double totalTime = 0;

        
        //file details
        public string custFileName;
        public string templateSize;

        // coordinates for raster lines
        public List<Point> pointList = new List<Point>();
        

        public materialPrice mPrice = new materialPrice();

        #endregion

        public zapQuote(int materialType, int templateSizeId, int mmId)
        {

            mPrice.getMaterialPrices(materialType, templateSizeId, mmId);

        }


        public void appendShapeData(double lengthVal, double areaVal, int zapType, double shapeHeight, double x, double y)
        {

            switch (zapType)
            {
                case zapConstants.vectorCut:
                    totalCutLength += lengthVal;
                    totalCutArea += areaVal;
                    cutCount += 1;
                    break;

                case zapConstants.vectorLightEngrave:
                    totalLightEngrave += lengthVal;
                    lightEngraveCount += 1;
                    break;

                case zapConstants.vectorMedEngrave:
                    totalMedEngrave += lengthVal;
                    medEngraveCount += 1;
                    break;

                case zapConstants.vectorHeavyEngrave:
                    totalHeavyEngrave += lengthVal;
                    heavyEngraveCount += 1;
                    break;

                case zapConstants.rasterLightEngrave:
                    totalLightFill += calcFillTime(areaVal, shapeHeight);
                    rasterArea += areaVal;
                    lightFillCount += 1;
                    break;

                case zapConstants.rasterMedEngrave:
                    totalMedFill += calcFillTime(areaVal, shapeHeight);
                    rasterArea += areaVal;
                    medFillCount += 1;
                    break;

                case zapConstants.rasterHeavyEngrave:
                    totalHeavyFill += calcFillTime(areaVal, shapeHeight);
                    rasterArea += areaVal;
                    heavyFillCount += 1;
                    break;

                case zapConstants.rasterLineShape:
                    totalHeavyFill += calcFillTime(areaVal, shapeHeight);
                    rasterArea += areaVal;
                    heavyFillCount += 1;
                    break;

                case zapConstants.rasterLine:
                    ////add coordinates

                    pointList.Add(new Point(x, y));
                    tempRasterHeight = shapeHeight;
                    break;
            }

        }


        public void calcTotalTime(int materialType, int templateSizeId, int mmId, double extraRaster, double extraHeight)
        {

            totalLengthEngrave = totalLightEngrave + totalMedEngrave + totalHeavyEngrave;

            // for debugging only 
            rasterArea += extraRaster;

            if (extraRaster > 0)
            {
                if (extraHeight == 0)
                {
                    extraHeight = tempRasterHeight;
                }
                extraRaster = calcFillTime(extraRaster, extraHeight);
            }

            totalAreaFill = totalLightFill + totalMedFill + totalHeavyFill + extraRaster;
            
            cutTime = totalCutLength * mPrice.timeCutMm;    
        

            //need to get raster engrave details
            // NOWDONE - test
            fillTime = totalAreaFill;


            engraveTime = totalLengthEngrave * mPrice.timeVectorEngraveMm;
            totalTime = cutTime + engraveTime + fillTime;

            
        }


        private double calcFillTime(double sArea, double sHeight)
        {
            double passes = sHeight / zapConstants.rasterEngraveWidth;
            double rasterLength = sArea / zapConstants.rasterEngraveWidth;

            return (rasterLength * zapConstants.rasterEngravePerMM) + ((passes * 2) * zapConstants.rasterTurnTime);
            //return (rasterLength * mPrice.timeRasterEngraveMm) + ((passes * 2) * zapConstants.rasterTurnTime);

        }

       
    }
}