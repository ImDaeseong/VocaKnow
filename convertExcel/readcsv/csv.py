import openpyxl

if __name__ == '__main__':

    file = open('E:/kata.txt', 'w', encoding='utf8')

    xls = openpyxl.load_workbook("aa.xlsx")
    sheet = xls["Sheet1"]

    for row in sheet.rows:

        index = row[0].row
        # print(index)
        if index == 1:
            continue

        sPart1 = row[1].value
        sPart2 = row[2].value
        sPart3 = row[3].value
        sKorea = row[4].value
        sIndo = row[5].value
        sPron = row[6].value
        # print('index:{}|sPart2:{}:sPart3:{}:sKorea:{}:sIndo:{}:sPron:{}'.format(index - 1, sPart2, sPart3, sKorea,
        # sIndo, sPron))

        file.write(str('{}|{}|{}|{}|{}|{}\n'.format(index - 1, sPart2, sPart3, sKorea, sIndo, sPron)))

    file.close()
