SELECT sellers.Surname as 'Фамилия', sellers.Name as 'Имя', SUM(s.Quantity) as 'Количество' FROM sellers
JOIN sales s on s.IDSel = sellers.ID
WHERE s.Date BETWEEN '2013-10-01' AND '2013-10-08'
GROUP BY sellers.ID
ORDER BY sellers.Surname, sellers.Name