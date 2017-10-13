// Readcsv
package main

import (
	"fmt"
	"io"
	"os"
	"xlsx" //https://github.com/tealeg/xlsx
)

func WriteKataKataString(sPath, sText string) {

	file, err := os.OpenFile(sPath, os.O_RDWR|os.O_APPEND, 0660)
	if os.IsNotExist(err) {
		file, err = os.Create(sPath)
	}
	defer file.Close()

	if err != nil {
		return
	}

	n, err := io.WriteString(file, sText)
	if err != nil {
		fmt.Println(n, err)
		return
	}
}

func getReadBahasa() {

	sSavePath := fmt.Sprintf("D:\\test\\kata.txt")

	sFilePath := "D:\\test\\자주쓰는외국어_인도네시아_2017년.xlsx"
	xlFile, err := xlsx.OpenFile(sFilePath)
	if err != nil {
		fmt.Printf("Read Error ", err)
	}

	for _, sheet := range xlFile.Sheets {
		for rIndex, row := range sheet.Rows {

			if rIndex == 0 {
				//제목 row
				continue
			}

			cell1, _ := row.Cells[2].String()
			cell2, _ := row.Cells[3].String()
			cell3, _ := row.Cells[4].String()
			cell4, _ := row.Cells[5].String()
			cell5, _ := row.Cells[6].String()
			sLine := fmt.Sprintf("%d|%s|%s|%s|%s|%s", rIndex, cell1, cell2, cell3, cell4, cell5)
			fmt.Println(sLine)

			WriteKataKataString(sSavePath, sLine+"\n")
		}
	}
}

func main() {

	getReadBahasa()
}
