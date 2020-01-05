(defblock :name curl :is-top t
	(worker
		:worker-name curl
		:verbs "curl"
	(path
		:alias url
	)
	(key-value-pair
		:alias method
		:key-names "--method" "-m"
		:key-values (choice :classes term :values "post" "get" "put" "delete")
		:is-single t)
	(end)
)
