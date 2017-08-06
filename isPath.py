#import sys, clr
    
#clr.AddReference('System.Web')
#from System.Web.HttpContext import Current
#sys.path.append(Current.Server.MapPath('pythonMod'))

import sys

sys.path.append('C:/python27/lib')

import measure

def Simple(pathdata):	
	
	p = measure.cubicsuperpath.parsePath(pathdata)
	stotal,x0,y0 = measure.csparea(p)
	factor = 1.0/3.5433070866
	stotal *= factor*1
	prec = 2
	scale = 1.0    
	lenstr = round(stotal*factor*scale,prec)
	
	return lenstr	