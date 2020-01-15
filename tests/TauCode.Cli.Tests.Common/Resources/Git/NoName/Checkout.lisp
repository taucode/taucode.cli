(defblock :name checkout :is-top t
	(worker
		:worker-name checkout
		:verbs "checkout"
		:doc "Checkout a branch."
		:usage-samples (
			"<todo>"
			"<todo>"
			))
	(idle :name args)
	(alt
		(key-value-pair
			:alias connection
			:key-names "--conn" "-c"
			:key-values (choice :classes string path :values *))

		(key-value-pair
			:alias provider
			:key-names "--provider" "-p"
			:key-values (choice :classes term :values "sqlserver" "postgresql"))

		(key-value-pair
			:alias exclude
			:key-names "--exclude" "-e"
			:key-values (choice :classes string term :values *))

	)
	(idle :links args next)
	(end)
)