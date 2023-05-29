SELECT	p.Name as 'Продукт',
	se.Surname as 'Фамилия', 
	se.Name as 'Имя', 
        SUM(sa.Quantity)/
		(SELECT SUM(Quantity) FROM sales
		join products on products.ID = sales.IDProd
		WHERE products.ID = p.ID
		GROUP BY IDProd) as 'Процент продаж'
FROM sellers se
JOIN sales sa on sa.IDSel = se.ID
JOIN products p on p.ID = sa.IDProd
JOIN arrivals a on a.IDProd = p.ID
WHERE 
	1=1
	AND (sa.Date BETWEEN '2013-10-01' AND '2013-10-08')
	AND (a.Date BETWEEN '2013-09-07' AND '2013-10-08')
GROUP BY se.ID, se.Surname, se.Name, p.ID
ORDER BY p.Name, se.Surname, se.Name