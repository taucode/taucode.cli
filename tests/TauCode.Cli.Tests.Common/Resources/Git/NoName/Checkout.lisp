(defblock :name checkout :is-top t
	(worker
		:worker-name checkout
		:verbs "checkout"
		:doc "Checkout a branch."
		:usage-samples (
			"<todo>"
			"<todo>"
			))
	(alt		
		;;; checkout existing branch
		(seq
			(opt
				(seq
					(idle :name existing-branch-options)
					(alt
						(multi-text
							:classes key
							:values "-q" "--quiet"
							:alias quiet
							:action option)	

						(multi-text
							:classes key integer-text
							:values "-2" "--ours"
							:alias ours
							:action option)

						(multi-text
							:classes key integer-text
							:values "-3" "--theirs"
							:alias theirs
							:action option)

						(fallback :name bad-key-fallback)
					)
					(idle :links existing-branch-options next)
					(idle)
				)
			)
			(some-text
				:name existing-branch-node
				:classes path
				:alias existing-branch
				:action argument
			)
		)
		
		;;; checkout new branch
		(seq
			(exact-text
				:classes key
				:value "-b"
				:alias new-branch
				:action option)
			(some-text
				:classes path
				:alias new-branch-name
				:action argument
			)
			(some-text
				:classes path
				:alias base-branch-name
				:action argument
			)
		)
	)
	(end)
)
