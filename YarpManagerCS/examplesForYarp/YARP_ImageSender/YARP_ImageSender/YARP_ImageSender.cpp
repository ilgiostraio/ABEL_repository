// YARP_ImageSender.cpp : Defines the entry point for the console application.
//

//#include "stdafx.h"

#include "../../include/opencv2/opencv.hpp"
#include <stdio.h>

#include <yarp/sig/all.h>
#include <yarp/os/all.h>
#include <yarp/dev/PolyDriver.h>
#include <yarp/dev/FrameGrabberInterfaces.h>

using namespace yarp::os;
using namespace yarp::sig;
using namespace yarp::dev;

int main()
{
	Network *yarp; // set up yarp
	BufferedPort<ImageOf<PixelRgb>> *imagePortOut;  // make a port for reading images

	yarp = new Network();
	imagePortOut = new BufferedPort< ImageOf<PixelRgb> >();
	imagePortOut->open("/imageOut");  // give the port a name

	CvCapture* capture = cvCaptureFromCAM(0);
	cv::Mat frame, frameCopy;
	int key = 0;
	IplImage *iplFrame;

	if( capture ) 
	{	
		for(;;)
		{
			iplFrame = cvQueryFrame( capture );

			frameCopy;
			frame = iplFrame;
			if( frame.empty() )
				return 0;
			if( iplFrame->origin == IPL_ORIGIN_TL )
				frame.copyTo( frameCopy );
			else
				flip( frame, frameCopy, 0 );

			if (iplFrame->channelSeq[0]=='B') 
			{
				cvCvtColor(iplFrame, iplFrame, CV_BGR2RGB);
			}

			ImageOf<PixelRgb> &yarpImage = imagePortOut->prepare();
			yarpImage.resize(iplFrame->width, iplFrame->height);
			yarpImage.wrapIplImage(iplFrame);
			imagePortOut->write();

			//cv::imshow( "result", frameCopy );
			
			//if( cv::waitKey( 10 ) >= 0 )
			//	break;
		}
	}
	
	cvReleaseImage(&iplFrame);
	cvReleaseCapture( &capture );
	cvDestroyWindow("result");

	return 0;
}


//int main() 
//{
//	Network yarp; // set up yarp
//	BufferedPort<ImageOf<PixelRgb> > imagePortOut;  // make a port for reading images
//	imagePortOut.open("/imageOut");  // give the port a name
//
//	ImageOf<PixelRgb> yarpImage;
//
//	CvCapture* capture = cvCaptureFromCAM(0);
//	cv::Mat frame, frameCopy;
//	int key = 0;
//
//	if( capture ) 
//	{	
//		for(;;)
//		{
//			IplImage* iplFrame = cvQueryFrame( capture );
//
//			// Resize the output image, this should not result in new
//			// memory allocation if the image is already the correct size
//			yarpImage.resize(iplFrame->width, iplFrame->height);
//
//			// Get an IplImage, the Yarp Image owns the memory pointed to
//			IplImage * iplImage = (IplImage*)yarpImage.getIplImage();
//
//			// Copy the captured image to the output image, flipping it if
//			// the coordinate origin is not the top left
//			if (IPL_ORIGIN_TL == iplFrame->origin)
//				cvCopy(iplFrame, iplImage, 0);
//			else
//				cvFlip(iplFrame, iplImage, 0);
//
//			if (iplFrame->channelSeq[0]=='B') {
//				cvCvtColor(iplImage, iplImage, CV_BGR2RGB);
//			}
//
//			cvNamedWindow("test", 1);
//			cvShowImage("test", iplImage);
//		}
//	}		
//	
//	cvReleaseCapture( &capture );
//    cvDestroyWindow("Face Analyzer");
//
//    cv::waitKey(0);
//    return 0;
//}


