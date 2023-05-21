import math
def main():
    showMenu()


def getLengthAndHeight():
    length=input("Enter length of tower\n")
    height=input("Enter height of tower\n")
    return length, height


def printTriangle(length, height):
    if  not(length%2) or length/height>2 :
        print("Sorry, can not print the triangle")
    else:
        odds=math.floor(length/2)-1
        print(odds)
        rowsOfKind=round((height-2)/odds)
        print(rowsOfKind)
        mod=(height-2)%odds
        print(mod)
        maxSpaces=round(length/2)
        for i in range(odds+2):
            if i==0:
                #print(maxSpaces)
                print(" "*(maxSpaces), "*")
            elif i==odds+1:
                print(" "*(maxSpaces-i),"*"*length)
            else:
                if i==1:
                    iterations=rowsOfKind+mod
                else:
                    iterations=rowsOfKind
                #print(maxSpaces - i)
                for j in range(iterations):
                    print(" "*(maxSpaces-i), "*"*(i*2+1))





def showMenu():
    print("Welcome to twiter tower!")
    choice=input("Enter 1 for rectangle tower, 2 for triangle tower and 3 for exit\n")
    if choice=='1':
        length, height=getLengthAndHeight()
        if length==height or abs(float(length)-float(height))>5:
            area=float(length)*float(height)
            print("The area of your tower is: ", area)
        else:
            scope=float(length)*2+float(height)*2
            print("The scope of your tower is: ", scope)
        showMenu()
    elif choice=='2':
        length, height = getLengthAndHeight()
        triangleChoice=input("Enter 1 if you want to get the scope of the triangle, 2 if you want printing\n")
        if triangleChoice=='1':
            pytagorasAns=math.sqrt((float(length)/2)**2+float(height)**2)
            scope=pytagorasAns*2+float(length)
            print("The scope of your triangle is: ", scope)
        elif triangleChoice=='2':
             printTriangle(int(length),int(height))
        else:
            print("illegal input")
            return
        showMenu()
    elif choice=='3':
        print("Good Bye!!\n")
        return
    else:
        print("illegal input")
        return

if __name__=='__main__':
    main()