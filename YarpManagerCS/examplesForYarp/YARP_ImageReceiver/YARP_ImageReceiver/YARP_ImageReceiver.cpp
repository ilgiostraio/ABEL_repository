// YARP_ImageReceiver.cpp : Defines the entry point for the console application.
//

//#include "stdafx.h"

#include "../../include/opencv2/opencv.hpp"
#include <stdio.h>
 
#include <yarp/os/all.h>
#include <yarp/sig/all.h>
#include <yarp/os/Bottle.h>
#include <iostream>
using namespace yarp::os;
using namespace yarp::sig;
using namespace std;
using namespace System;
using namespace System::Runtime::InteropServices;

int main() {
    Network *yarp; // set up yarp
    //BufferedPort<ImageOf<PixelRgb>> *imagePortIn;  // make a port for reading images
    BufferedPort<Bottle>*imagePortIn;
	yarp = new Network();
	imagePortIn = new BufferedPort<Bottle>;
	imagePortIn->open("/imageIn");  // give the port a name

	if (yarp->checkNetwork()) cout << "YARP Network exists" << endl;
	if (!yarp->exists("/imageOut"))
		cout << "/imageOut port DOESN'T exist" << endl;

	yarp->connect("/imageOut","/imageIn");

	IplImage* iplImage;
	cv::Mat frame, frameCopy;
    for (;;) 
	{ 
		 Bottle* yarpImage = imagePortIn->read();
      //ImageOf<PixelRgb> *yarpImage = imagePortIn->read();  // read an image
	  if (yarpImage != NULL)
	  {
		  char* str2 = (char*)Marshal::StringToHGlobalAnsi(yarpImage).ToPointer();
			Marshal::FreeHGlobal((IntPtr)str2);

			 IplImage *inputImg= cvCreateImage(cvSize(640,480), IPL_DEPTH_8U, 3); 
       inputImg->imageData = (char *)image;
		  iplImage = (IplImage*)yarpImage->getIplImage();
		  /*IplImage *cvImage = cvCreateImage(cvSize(yarpImage->width(), yarpImage->height()), IPL_DEPTH_8U, 3 );
		  cvCvtColor((IplImage*)yarpImage->getIplImage(), cvImage, CV_RGB2BGR);
			*/
		  // Copy the captured image to the output image, flipping it if
		  // the coordinate origin is not the top left
		  /*if (IPL_ORIGIN_TL == iplImage->origin)
			  cvCopy(iplImage, iplImage, 0);
		  else
			  cvFlip(iplImage, iplImage, 0);

		  if (iplImage->channelSeq[0]=='B') {
			  cvCvtColor(iplImage, iplImage, CV_BGR2RGB);
		  }*/

		  frameCopy;
		  frame = iplImage;
		  if( frame.empty() )
			  return 0;
		  if( iplImage->origin == IPL_ORIGIN_TL )
			  frame.copyTo( frameCopy );
		  else
			  flip( frame, frameCopy, 0 );			

		  
		  cv::imshow( "Image through YARP", frameCopy );

		  if( cv::waitKey( 10 ) >= 0 )
				break;
      }
    }

	cvReleaseImage(&iplImage);
	cvDestroyWindow("Image through YARP");

    return 0;
  }

